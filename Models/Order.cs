namespace ProductionGrade.Models
{
    public class Order : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
    }

}
