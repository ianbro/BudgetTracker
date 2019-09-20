using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BudgetTracker.Data.EntityFramework.Models
{
    public class BudgetModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double? PercentAmount { get; set; }
        public decimal? SetAmount { get; set; }
        public DateTime BudgetStart { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal FundBalance { get; set; }

        public Guid OwnerId { get; set; }
        [JsonIgnore]
        [ForeignKey("OwnerId")]
        public UserModel Owner { get; set; }

        public Guid? ParentBudgetId { get; set; }
        [JsonIgnore]
        [ForeignKey("ParentBudgetId")]
        public BudgetModel ParentBudget { get; set; }

        public Guid DurationId { get; set; }
        [ForeignKey("DurationId")]
        public BudgetDurationModel Duration { get; set; }
    }
}