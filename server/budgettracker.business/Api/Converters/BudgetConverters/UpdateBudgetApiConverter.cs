using budgettracker.common.Models;
using budgettracker.business.Api.Contracts.BudgetApi.UpdateBudget;
using budgettracker.common.Models.BudgetDurations;
using budgettracker.business.Api.Contracts.BudgetApi.BudgetDurations;
using budgettracker.common.Exceptions;

namespace budgettracker.business.Api.Converters.BudgetConverters
{
    public class UpdateBudgetApiConverter
    {
        public static Budget ToModel(UpdateBudgetRequestContract requestContract)
        {
            return new Budget()
            {
                Id = requestContract.Id,
                Name = requestContract.Name,
                SetAmount = requestContract.SetAmount,
                Duration = GeneralBudgetApiConverter.GetBudgetDuration(requestContract.Duration),
                ParentBudgetId = requestContract.ParentBudgetId,
                BudgetStart = requestContract.BudgetStart
            };
        }

        public static Budget ToModel(UpdateBudgetResponseContract responseContract)
        {
            throw new System.NotImplementedException();
        }

        public static UpdateBudgetRequestContract ToRequestContract(Budget model)
        {
            throw new System.NotImplementedException();
        }

        public static UpdateBudgetResponseContract ToResponseContract(Budget model)
        {
            return new UpdateBudgetResponseContract()
            {
                Id = model.Id,
                Name = model.Name,
                SetAmount = model.SetAmount,
                Duration = GeneralBudgetApiConverter.GetBudgetDuration(model.Duration),
                BudgetStart = model.BudgetStart,
                ParentBudgetId = model.ParentBudgetId,
            };
        }
    }
}
