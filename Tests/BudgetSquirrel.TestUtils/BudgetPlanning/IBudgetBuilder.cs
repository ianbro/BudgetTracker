using System;
using BudgetSquirrel.Business;
using BudgetSquirrel.Business.BudgetPlanning;

namespace BudgetSquirrel.TestUtils.Budgeting
{
    public interface IBudgetBuilder
    {
        IBudgetBuilder SetFund(Func<IFundBuilder, IFundBuilder> fundBuilder);

        IBudgetBuilder SetParentBudget(Budget budget);

        /// <summary>
        /// Set the percent amount for this budget. If you set the percentAmount,
        /// this will take priority over the set amount in most cases. So you
        /// may need to call SetFixedAmount(null) on this builder to clear it.
        /// </summary>
        IBudgetBuilder SetPercentAmount(double? percentAmount);

        /// <summary>
        /// Set the fixed amount for this budget. If you want this budget to
        /// be seen as a fixed amount budget, you will need to clear the percent
        /// amount by calling SetPercentAmount(null) on this builder. This is
        /// because percent amount is given priority over set amount in most
        /// cases of calculation.
        /// </summary>
        IBudgetBuilder SetFixedAmount(decimal? setAmount);

        IBudgetBuilder SetBudgetPeriod(BudgetPeriod budgetPeriod);

        Budget Build();
    }
}
