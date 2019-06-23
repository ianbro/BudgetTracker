using budgettracker.business.Api.Contracts.Requests;
using budgettracker.business.Api.Contracts.Responses;
using budgettracker.business.Api.Interfaces;
using budgettracker.data.Repositories.Interfaces;
using budgettracker.common.Models;
using budgettracker.data.Repositories;
using budgettracker.business.Api.Converters.BudgetConverters;
using budgettracker.business.Api.Contracts.BudgetApi.CreateBudget;
using budgettracker.business.Api.Contracts.BudgetApi.DeleteBudgets;
using budgettracker.data.Exceptions;
using budgettracker.common;
using budgettracker.business.Api.Contracts.BudgetApi;
using budgettracker.business.Api.Contracts.BudgetApi.UpdateBudget;

using GateKeeper.Configuration;
using GateKeeper.Cryptogrophy;

using Microsoft.Extensions.Configuration;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace budgettracker.business.Api
{
    public class BudgetApi :  ApiBase<User>, IBudgetApi
    {

        private readonly IBudgetRepository _budgetRepository;

        public BudgetApi(IBudgetRepository budgetRepository, IConfiguration appConfig, UserRepository userRepository)
            : base(userRepository, new Rfc2898Encryptor(),
                    ConfigurationReader.FromAppConfiguration(appConfig))
        {
            _budgetRepository = budgetRepository;
        }

        public async Task<ApiResponse> CreateBudget(ApiRequest request)
        {
            User user = await Authenticate(request);

            CreateBudgetArgumentApiContract budgetRequest = request.Arguments<CreateBudgetArgumentApiContract>();

            if(!Validation.IsCreateBudgetRequestValid(budgetRequest.BudgetValues))
            {
                return new ApiResponse(Constants.Budget.ApiResponseErrorCodes.INVALID_ARGUMENTS);
            }

            Budget newBudget = CreateBudgetApiConverter.ToModel(budgetRequest.BudgetValues);
            newBudget.Owner = user;

            try
            {
                newBudget = await _budgetRepository.CreateBudget(newBudget);
            }
            catch (RepositoryException ex)
            {
                return new ApiResponse(ex.Message);
            }

            CreateBudgetResponseContract response = CreateBudgetApiConverter.ToResponseContract(newBudget);

            return new ApiResponse(response);
        }

        public async Task<ApiResponse> UpdateBudget(ApiRequest request)
        {
            await Authenticate(request);

            UpdateBudgetArgumentApiContract budgetRequest = request.Arguments<UpdateBudgetArgumentApiContract>();

            Budget newBudget = UpdateBudgetApiConverter.ToModel(budgetRequest.BudgetValues);

            if(!Validation.IsUpdateBudgetRequestValid(budgetRequest.BudgetValues))
            {
                return new ApiResponse(Constants.Budget.ApiResponseErrorCodes.INVALID_ARGUMENTS);
            }

            try
            {
                newBudget = await _budgetRepository.UpdateBudget(newBudget);
            }
            catch (RepositoryException ex)
            {
                return new ApiResponse(ex.Message);
            }

            UpdateBudgetResponseContract response = UpdateBudgetApiConverter.ToResponseContract(newBudget);

            return new ApiResponse(response);
        }

        public async Task<ApiResponse> DeleteBudgets(ApiRequest request)
        {
            await Authenticate(request);

            ApiResponse response = null;

            DeleteBudgetArgumentsApiContract deleteArgs = request.Arguments<DeleteBudgetArgumentsApiContract>();
            try
            {
                await _budgetRepository.DeleteBudgets(deleteArgs.BudgetIds);
                response = new ApiResponse();
            }
            catch (RepositoryException e)
            {
                response = new ApiResponse(e.Message);
            }

            return response;
        }

        public async Task<ApiResponse> GetRootBudgets(ApiRequest request)
        {
            User user = await Authenticate(request);
            ApiResponse response;

            List<Budget> rootBudgets = await _budgetRepository.GetRootBudgets(user.Id.Value);
            List<BudgetResponseContract> rootBudgetContracts = GeneralBudgetApiConverter.ToGeneralResponseContracts(rootBudgets);
            BudgetListResponseContract responseData = new BudgetListResponseContract()
            {
                Budgets = rootBudgetContracts
            };

            response = new ApiResponse(responseData);
            return response;
        }
    }
}
