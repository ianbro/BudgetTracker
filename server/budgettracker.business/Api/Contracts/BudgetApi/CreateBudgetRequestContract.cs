using System;
using Newtonsoft.Json;

namespace budgettracker.business.Api.Contracts.BudgetApi
{
    public class CreateBudgetRequestContract : IApiContract {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("set-amount")]
        public decimal SetAmount { get; set; }

        /// <summary>
        /// <p> The duration of the budget cycle in days.</p>
        /// </summary>    
        [JsonProperty("duration")]
        public int Duration { get; set; }

        /// <summary>
        /// <p>
        /// Allows the user to set a start time, if null the start time will be today.
        /// </p>
        /// </summary>
        public DateTime? BudgetStart { get; set; }

        /// <summary>
        /// <p> The guid the of the parent budget, if the budget is the 'root' 
        /// this will be null. This string is converted to guid later. </p>
        /// </summary>
        [JsonProperty("parent-id")]
        public Guid? ParentBudgetId { get; set; }
    }
}