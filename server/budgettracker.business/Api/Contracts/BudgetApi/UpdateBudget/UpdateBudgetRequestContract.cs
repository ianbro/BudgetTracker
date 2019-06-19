using System;
using budgettracker.business.Api.Contracts.BudgetApi.BudgetDurations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace budgettracker.business.Api.Contracts.BudgetApi.UpdateBudget
{
    public class UpdateBudgetRequestContract : IApiContract {

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("set-amount")]
        public decimal SetAmount { get; set; }

        /// <summary>
        /// <p> The duration of the budget cycle in days or months.</p>
        /// </summary>
        [JsonProperty("duration")]
        public JObject DurationTemp { get; set; }

        public BudgetDurationBaseContract Duration {
            get {
                string durationSerialized = JsonConvert.SerializeObject(DurationTemp);
                if (DurationTemp.ContainsKey("start-day-of-month") &&
                    DurationTemp.ContainsKey("end-day-of-month"))
                {
                    return JsonConvert.DeserializeObject<MonthlyBookEndedDurationContract>(durationSerialized);
                }
                else if (DurationTemp.ContainsKey("number-days"))
                {
                    return JsonConvert.DeserializeObject<MonthlyDaySpanDurationContract>(durationSerialized);
                }
                else
                {
                    throw new JsonSerializationException("Could not understand the duration request.");
                }
            }
        }

        [JsonProperty("budget-start")]
        public DateTime BudgetStart { get; set; }

        /// <summary>
        /// <p> The guid the of the parent budget, if the budget is the 'root' 
        /// this will be null. This string is converted to guid later. </p>
        /// </summary>
        [JsonProperty("parent-id")]
        public Guid? ParentBudgetId { get; set; }
    }
}