namespace ProductionGrade.DTOs
{
    public class CreateOrderDto
    {
        public Guid UserId { get; set; }
        public List<CreateOrderLineDto> OrderLines { get; set; } = new();
    }

    public class CreateOrderLineDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }

}
