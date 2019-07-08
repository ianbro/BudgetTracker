using BudgetTracker.Business.Api.Contracts;
using BudgetTracker.Business.Api.Contracts.AuthenticationApi;
using BudgetTracker.Common;
using BudgetTracker.Common.Models;
using System;

namespace BudgetTracker.Business.Api.Converters
{
    public class UserApiConverter : IApiConverter<User, UserRequestApiContract, UserResponseApiContract>
    {
        public User ToModel(UserRequestApiContract contract)
        {
            User user = new User() {
                Id = contract.Id,
                FirstName = contract.FirstName,
                LastName = contract.LastName,
                Username = contract.UserName,
                Password = contract.Password,
                Email = contract.Email
            };
            return user;
        }

        public User ToModel(UserResponseApiContract contract)
        {
            User user = new User() {
                Id = contract.Id,
                FirstName = contract.FirstName,
                LastName = contract.LastName,
                Username = contract.UserName,
                Email = contract.Email
            };
            return user;
        }

        public UserRequestApiContract ToRequestContract(User model)
        {
            UserRequestApiContract contract = new UserRequestApiContract() {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Password = model.Password,
                Email = model.Email
            };
            return contract;
        }

        public UserResponseApiContract ToResponseContract(User model)
        {
            UserResponseApiContract contract = new UserResponseApiContract() {
                Id = model.Id.Value,
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Username,
                Email = model.Email
            };
            return contract;
        }
    }
}