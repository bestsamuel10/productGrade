using FluentValidation;
using ProductionGrade.DTOs;

namespace ProductionGrade.Validation
{
    public class OrderValidator : AbstractValidator<CreateOrderDto>
    {
        public OrderValidator()
        {
            RuleFor(o => o.UserId).NotEmpty(); // Guid must not be default
            RuleForEach(o => o.OrderLines).SetValidator(new OrderLineValidator());
        }
    }

    public class OrderLineValidator : AbstractValidator<CreateOrderLineDto>
    {
        public OrderLineValidator()
        {
            RuleFor(ol => ol.ProductId).NotEmpty(); // Guid must not be default
            RuleFor(ol => ol.Quantity).GreaterThan(0);
        }
    }

}
