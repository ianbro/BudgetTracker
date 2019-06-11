using budgettracker.common.Models;
using budgettracker.business.Api.Contracts.BudgetApi.UpdateBudget;
using System;

namespace budgettracker.business.Api.Converters.BudgetConverters
{
    public class UpdateBudgetApiConverter : IApiConverter<Budget, UpdateBudgetRequestContract, UpdateBudgetResponseContract>
    {        public Budget ToModel(UpdateBudgetRequestContract requestContract)
        {
            return new Budget()
            {
                Id = requestContract.Id,
                Name = requestContract.Name,
                SetAmount = requestContract.SetAmount,
                Duration = requestContract.Duration,
                ParentBudgetId = requestContract.ParentBudgetId,
                BudgetStart = requestContract.BudgetStart
            };
        }

        public Budget ToModel(UpdateBudgetResponseContract responseContract)
        {
            throw new System.NotImplementedException();
        }

        public UpdateBudgetRequestContract ToRequestContract(Budget model)
        {
            throw new System.NotImplementedException();
        }

        public UpdateBudgetResponseContract ToResponseContract(Budget model)
        {
            return new UpdateBudgetResponseContract()
            {
                Id = model.Id,
                Name = model.Name,
                SetAmount = model.SetAmount,
                Duration = model.Duration,
                BudgetStart = model.BudgetStart,
                ParentBudgetId = model.ParentBudgetId,    
            };
        }
    }
}