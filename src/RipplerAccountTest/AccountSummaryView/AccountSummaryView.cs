using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RipplerAccountTest.AccountSummaryView
{
    [Table("AccountSummaryView")]
    public class AccountSummaryView
    {
        [Key]
        public Guid AggregateId { get; set; }
        public int Version { get; set; }
        public string FriendlyName { get; set; }
        public double Balance { get; set; }
    }
}