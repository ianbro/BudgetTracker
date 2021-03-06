using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BudgetSquirrel.Business.Tracking;

namespace BudgetSquirrel.Business.BudgetPlanning
{
  public class BudgetLoader
  {
    private IUnitOfWork unitOfWork;

    public BudgetLoader(IUnitOfWork unitOfWork)
    {
      this.unitOfWork = unitOfWork;
    }

    public async Task<Fund> LoadFundTree(Fund root, BudgetPeriod budgetPeriod)
    {
      Fund rootWBudget = (await this.LoadCurrentBudgetForFunds(new Fund[] { root }, budgetPeriod)).First();
      rootWBudget.SubFunds = await this.LoadFundTreeRecursive(root, budgetPeriod);
      return rootWBudget;
    }
    
    public async Task<IEnumerable<Fund>> LoadCurrentBudgetForFunds(IEnumerable<Fund> funds, BudgetPeriod budgetPeriod)
    {
      IEnumerable<Guid> loadedFundsIds = funds.Select(x => x.Id);
      IEnumerable<Budget> budgets = await this.unitOfWork.GetRepository<Budget>()
                                                         .GetAll()
                                                         .Include(b => b.BudgetPeriod)
                                                         .Where(b => loadedFundsIds.Contains(b.FundId))
                                                         .ToListAsync();

      budgets = budgets.Where(b => b.BudgetPeriod.StartDate.Date == budgetPeriod.StartDate.Date &&
                                   b.BudgetPeriod.EndDate.Date == budgetPeriod.EndDate.Date);
                                                        //  .ToListAsync();

      // Put the budgets on their corresponding fund. This is equivelant to:
      // loadedSubFunds = loadedSubFunds inner join budgets on budget.FundId = fund.Id
      // There should only be one budget per fund, otherwise, this will bug out.
      funds = funds.Join(
        budgets,
        f => f.Id,
        b => b.FundId,
        (f, b) => {
          b.Fund = f;
          f.HistoricalBudgets = new List<Budget>() { b };
          return f;
        });

      return funds;
    }

    private async Task<IEnumerable<Fund>> LoadFundTreeRecursive(Fund root, BudgetPeriod budgetPeriod)
    {
      IEnumerable<Fund> loadedSubFunds = await this.unitOfWork.GetRepository<Fund>()
                                                  .GetAll()
                                                  .Include(f => f.Duration)
                                                  .Where(b => b.ParentFundId == root.Id)
                                                  .ToListAsync();

      loadedSubFunds = await this.LoadCurrentBudgetForFunds(loadedSubFunds, budgetPeriod);

      foreach (Fund subFund in loadedSubFunds)
      {
        subFund.SubFunds = await LoadFundTreeRecursive(subFund, budgetPeriod);
      }
      return loadedSubFunds;
    }
  }
}