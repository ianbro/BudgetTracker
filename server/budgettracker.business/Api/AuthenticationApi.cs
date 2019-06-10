using budgettracker.business.Api.Contracts;
using budgettracker.business.Api.Contracts.AuthenticationApi;
using budgettracker.business.Api.Contracts.Responses;
using budgettracker.business.Api.Contracts.Requests;
using budgettracker.business.Api.Converters;
using budgettracker.business.Authentication;
using budgettracker.common;
using budgettracker.common.Authentication;
using budgettracker.common.Models;
using budgettracker.data;
using budgettracker.data.Repositories;
using GateKeeper.Configuration;
using GateKeeper.Cryptogrophy;
using GateKeeper.Exceptions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace budgettracker.business.Api
{
    /// <summary>
    /// <p>
    /// Provides an API for the authentication logic for this application. With
    /// this API, one can register new users, login, logout and more.
    /// </p>
    /// </summary>
    public class AuthenticationApi : ApiBase<User>
    {
        UserApiConverter _userApiConverter;

        public AuthenticationApi(UserRepository userRepository,
            IConfiguration appConfig)
            : base(userRepository, new Rfc2898Encryptor(),
                    ConfigurationReader.FromAppConfiguration(appConfig))
        {
            _userApiConverter = new UserApiConverter();
        }

        /// <summary>
        /// <p>
        /// Allows a user to register a new account.
        /// </p>
        /// </summary>
        public ApiResponse Register(ApiRequest request)
        {
            UserRegistrationArgumentApiContract arguments = request.Arguments<UserRegistrationArgumentApiContract>();
            UserRequestApiContract userValues = arguments.UserValues;
            User userModel = _userApiConverter.ToModel(userValues);
            ApiResponse response;

            IEnumerable<string> errors = null;
            if (!AccountValidation.IsAccountRegistrationRequestValid(userValues))
            {
                errors = new List<string>() {
                    AuthenticationConstants.ApiResponseErrorCodes.PASSWORD_CONFIRM_INCORRECT
                };
                response = new ApiResponse(String.Join(";", errors.ToArray()));
                return response;
            }
            if (((UserRepository) _userRepository).Register(userModel, out errors))
            {
                UserResponseApiContract responseData = _userApiConverter.ToResponseContract(userModel);
                response = new ApiResponse(responseData);
            }
            else
            {
                response = new ApiResponse(String.Join(";", errors.ToArray()));
            }

            return response;
        }

        /// <summary>
        /// <p>
        /// Authenticates the user, returning it in the response if authorized.
        /// </p>
        /// </summary>
        public async Task<ApiResponse> AuthenticateUser(ApiRequest request)
        {
            ApiResponse response;

            try {
                User authenticatedUser = await Authenticate(request);
                UserResponseApiContract responseData = _userApiConverter.ToResponseContract(authenticatedUser);
                response = new ApiResponse(responseData);
            }
            catch (AuthenticationException)
            {
                response = new ApiResponse("Those credentials could not be authorized.");
            }
            return response;
        }

        public async Task<ApiResponse> DeleteUser(ApiRequest request)
        {
            ApiResponse response;

            User authenticatedUser = await Authenticate(request);
            try
            {
                await ((UserRepository) _userRepository).Delete(authenticatedUser.Id);
                response = new ApiResponse();
            }
            catch (InvalidOperationException)
            {
                response = new ApiResponse("Could not find the specified user.");
            }
            return response;
        }
    }
}
