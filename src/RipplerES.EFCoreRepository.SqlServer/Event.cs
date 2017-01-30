using System;
using System.ComponentModel.DataAnnotations;

namespace RipplerES.EFCoreRepository
{
    public class Event
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Guid AggregateId { get; set; }
        [Required]
        public int Version { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Data { get; set; }
        public string MetaData { get; set; }
    }
}