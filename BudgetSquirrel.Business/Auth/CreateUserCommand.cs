using System;
using BudgetSquirrel.Business.BudgetPlanning;

namespace BudgetSquirrel.Business.Auth
{
  public class CreateUserCommand
  {
    private IAsyncQueryService asyncQueryService;
    private const string newUserRootBudgetName = "My Budget";
    private const decimal newUserRootBudgetFundBalance = 0;
    private const int newUserRootBudgetDurationEndDate = 31;
    private const bool newUserRootBudgetDurationShouldRollover = false;

    private string userName;
    private string firstName;
    private string lastName;
    private string email;

    public CreateUserCommand(IAsyncQueryService asyncQueryService, string username, string firstName, string lastName, string email)
    {
      this.asyncQueryService = asyncQueryService;
      this.userName = username;
      this.firstName = firstName;
      this.lastName = lastName;
      this.email = email;
    }

    public UserRootBudgetRelationship Run()
    {
      User user = new User(this.userName,
                            this.firstName,
                            this.lastName,
                            this.email);
      Budget rootBudget = CreateNewUserRootBudget(user);
      UserRootBudgetRelationship userWithBudget = new UserRootBudgetRelationship(user, rootBudget);
      return userWithBudget;
    }

    private Budget CreateNewUserRootBudget(User user)
    {
      BudgetDurationBase duration = new MonthlyBookEndedDuration(newUserRootBudgetDurationEndDate, newUserRootBudgetDurationShouldRollover);
      Budget rootBudget = new Budget(newUserRootBudgetName, newUserRootBudgetFundBalance, duration, DateTime.Now, user);

      return rootBudget;
    }
  }
}