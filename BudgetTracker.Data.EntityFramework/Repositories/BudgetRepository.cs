using BudgetTracker.Business.Budgeting;
using BudgetTracker.Business.BudgetPeriods;
using BudgetTracker.Business.Auth;
using BudgetTracker.Data.EntityFramework;
using BudgetTracker.Data.EntityFramework.Converters;
using BudgetTracker.Data.EntityFramework.Models;
using BudgetTracker.Business.Ports.Exceptions;
using BudgetTracker.Business.Ports.Repositories;

using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace BudgetTracker.Data.EntityFramework.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {

        private readonly BudgetTrackerContext _dbContext;

        public BudgetRepository(BudgetTrackerContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Budget> CreateBudget(Budget budget)
        {
            BudgetModel newBudget = BudgetConverter.ToDataModel(budget);

            await _dbContext.Budgets.AddAsync(newBudget);
            int recordSaved = await _dbContext.SaveChangesAsync();

            if(recordSaved < 1)
            {
                throw new RepositoryException("Could not save budget to database");
            }
            return BudgetConverter.ToBusinessModel(newBudget);
        }

        public async Task DeleteBudgets(List<Guid> Ids)
        {
            List<BudgetModel> toDelete = await _dbContext.Budgets.Where(b => Ids.Contains(b.Id)).ToListAsync();
            List<Guid> nonExistant = Ids.Where(id => !toDelete.Select(b => b.Id).Contains(id)).ToList();
            List<Guid> erroredBudgets = nonExistant;

            _dbContext.RemoveRange(toDelete);
            if (await _dbContext.SaveChangesAsync() < toDelete.Count())
            {
                List<Guid> notDeleted = await (from b in _dbContext.Budgets
                                                where Ids.Contains(b.Id)
                                                select b.Id).ToListAsync();
                erroredBudgets.AddRange(notDeleted);
            }
            if (erroredBudgets.Any())
            {
                string errorIds = String.Join(",", erroredBudgets.ToArray());
                throw new RepositoryException("NOT_DELETED:" + errorIds);
            }
        }

        public async Task<Budget> GetBudget(Guid id)
        {
            BudgetModel budget = await _dbContext.Budgets.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (budget == null)
            {
                throw new RepositoryException("Was not able to find a budget with the id " + id);
            }
            else
            {
                budget.Duration = await LoadDurationForBudget(budget);
                budget.Owner = await _dbContext.Users.SingleAsync(u => u.Id == budget.OwnerId);
                return BudgetConverter.ToBusinessModel(budget);
            }
        }

        public async Task<List<Budget>> GetRootBudgets(Guid userId)
        {
            List<BudgetModel> rootBudgets = await _dbContext.Budgets.Where(b => b.OwnerId == userId).ToListAsync();
            // Have to do this outside of query because EF can't do null checks.
            rootBudgets = rootBudgets.Where(b => b.ParentBudgetId == null).ToList();
            await LoadDurationsForBudgets(rootBudgets);
            List<Budget> rootBudgetsBusinessModels = BudgetConverter.ToBusinessModels(rootBudgets);
            return rootBudgetsBusinessModels;
        }

        private async Task<BudgetDurationModel> LoadDurationForBudget(BudgetModel budget)
        {
            BudgetDurationModel duration = await _dbContext.BudgetDurations.SingleAsync(d => d.Id == budget.DurationId);
            return duration;
        }

        private async Task LoadDurationsForBudgets(List<BudgetModel> budgets)
        {
            foreach (BudgetModel budgetModel in budgets)
            {
                budgetModel.Duration = await _dbContext.BudgetDurations.SingleAsync(d => d.Id == budgetModel.DurationId);
            }
        }

        public async Task<Budget> UpdateBudget(Budget budget)
        {
            BudgetModel oldBudget;

            oldBudget = await _dbContext.Budgets.SingleOrDefaultAsync(x => x.Id == budget.Id);

            if(oldBudget == null)
            {
                throw new RepositoryException("No Budget with the id: " + budget.Id + " was found.");
            }

            oldBudget.Name = budget.Name;
            oldBudget.PercentAmount = budget.PercentAmount;
            oldBudget.SetAmount = budget.SetAmount;
            oldBudget.FundBalance = budget.FundBalance;
            oldBudget.BudgetStart = budget.BudgetStart;
            oldBudget.ParentBudgetId = budget.ParentBudgetId;

            if (oldBudget.ParentBudgetId == null)
            {
                BudgetDurationModel oldDuration = await _dbContext.BudgetDurations.SingleOrDefaultAsync(d => d.Id == oldBudget.DurationId);
                SetNewBudgetDurationValues(oldDuration, budget.Duration);
                oldBudget.Duration = oldDuration;
            }

            int recordSaved = await _dbContext.SaveChangesAsync();

            return BudgetConverter.ToBusinessModel(oldBudget);
        }

        public async Task LoadSubBudgets(Budget budget, bool recursive=false)
        {
            List<BudgetModel> subBudgetsData = await _dbContext.Budgets.Where(b => b.ParentBudgetId.Value == budget.Id).ToListAsync();
            await LoadDurationsForBudgets(subBudgetsData);

            List<Budget> subBudgets = BudgetConverter.ToBusinessModels(subBudgetsData);
            foreach (Budget subBudget in subBudgets)
            {
                subBudget.ParentBudget = budget;
                if (recursive)
                {
                    await LoadSubBudgets(subBudget, recursive);
                }
            }
            budget.SubBudgets = subBudgets;
        }

        private void SetNewBudgetDurationValues(BudgetDurationModel original, BudgetDurationBase newBudget)
        {
            if (newBudget is MonthlyBookEndedDuration)
            {
                MonthlyBookEndedDuration newBookendDuration = (MonthlyBookEndedDuration) newBudget;
                original.DurationType = DataConstants.BudgetDuration.TYPE_MONTHLY_BOOKENDS;
                original.StartDayOfMonth = newBookendDuration.StartDayOfMonth;
                original.EndDayOfMonth = newBookendDuration.EndDayOfMonth;
                original.RolloverStartDateOnSmallMonths = newBookendDuration.RolloverStartDateOnSmallMonths;
                original.RolloverEndDateOnSmallMonths = newBookendDuration.RolloverEndDateOnSmallMonths;
                original.NumberDays = -1;
            }
            else if (newBudget is MonthlyDaySpanDuration)
            {
                MonthlyDaySpanDuration newDayspanDuration = (MonthlyDaySpanDuration) newBudget;
                original.DurationType = DataConstants.BudgetDuration.TYPE_MONTHLY_SPAN;
                original.StartDayOfMonth = -1;
                original.EndDayOfMonth = -1;
                original.RolloverStartDateOnSmallMonths = false;
                original.RolloverEndDateOnSmallMonths = false;
                original.NumberDays = newDayspanDuration.NumberDays;
            }
            else
            {
                throw new RepositoryException($"The Budget duration type {newBudget.GetType().ToString()} is not a supported type.");
            }
        }
    }
}