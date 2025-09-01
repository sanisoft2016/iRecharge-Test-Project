namespace iRechargeTestProject.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string Reference { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;// e.g., Pending, Completed
        public int MerchantId { get; set; }
        public Merchant? Merchant { get; set; }
        public int PaymentMethodId { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}