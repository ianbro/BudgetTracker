using BudgetTracker.Business.Api.Contracts.Responses;
using BudgetTracker.Business.Api.Contracts.Requests;
using BudgetTracker.Business.Api.Interfaces;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;
using System.Security.Authentication;
using System;
using BudgetTracker.Business.Api.Contracts.BudgetApi;

namespace BudgetTracker.Web.Controllers
{
    [Route("api/budget")]
    [ApiController]
    public class BudgetApiController : ControllerBase
    {

        private readonly IBudgetApi _budgetApi;

        public BudgetApiController(IBudgetApi budgetApi)
        {
            _budgetApi = budgetApi;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateBudget(ApiRequest request)
        {
            try
            {
                return new JsonResult(await _budgetApi.CreateBudget(request));
            }
            catch (AuthenticationException)
            {
                return Forbid();
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteBudgets(ApiRequest request)
        {
            try
            {
                return new JsonResult(await _budgetApi.DeleteBudgets(request));
            }
            catch (AuthenticationException)
            {
                return Forbid();
            }
        }

        [HttpPost("roots")]
        public async Task<IActionResult> GetRootBudget(ApiRequest request)
        {
            try
            {
                return new JsonResult(await _budgetApi.GetRootBudgets(request));
            }
            catch (AuthenticationException)
            {
                return Forbid();
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> UpdateBudget(ApiRequest request)
        {
            try
            {
                return new JsonResult(await _budgetApi.UpdateBudget(request));    
            }
            catch (AuthenticationException) 
            {
                return Forbid();
            }            
        }
    }
}