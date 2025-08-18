using System.ComponentModel.DataAnnotations;

namespace iRechargeTestProject.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string MessageTemplate { get; set; } = string.Empty;
        [MaxLength(128)]
        public string Level { get; set; } = string.Empty;
        public DateTimeOffset TimeStamp { get; set; }
        public string Exception { get; set; } = string.Empty;
        public string Properties { get; set; } = string.Empty; // JSONB can be mapped as string
    }
}