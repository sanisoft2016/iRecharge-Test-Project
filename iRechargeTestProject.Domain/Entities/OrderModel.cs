using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iRechargeTestProject.Domain.Entities
{
    public class OrderModel
    {
        public int Id { get; set; }

        [MaxLength(40)]
        public string CustomerId { get; set; } = string.Empty;
        [ForeignKey("CustomerId")]
        public ApplicationUser? User { get; set; } = null;

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductModel? Product { get; set; } = null;
        public int Quantity { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now.ToUniversalTime();
    }
}
