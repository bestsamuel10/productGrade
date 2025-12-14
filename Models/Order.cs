namespace ProductionGrade.Models
{
    public class Order
    {
        public int Id { get; set; }                // Primary Key
        public int UserId { get; set; }            // For future authentication
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }

        // Navigation property
        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }
}
