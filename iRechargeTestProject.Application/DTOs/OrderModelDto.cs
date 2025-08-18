using iRechargeTestProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Application.DTOs
{
    public class OrderModelDto
    {
        public int Id { get; set; }
        public string CustomerId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
