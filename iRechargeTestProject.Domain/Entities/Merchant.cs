namespace iRechargeTestProject.Domain.Entities
{
    public class Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<Payment>? Payments { get; set; }
    }
}