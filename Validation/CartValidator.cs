using FluentValidation;
using ProductionGrade.DTOs;


namespace ProductionGrade.Validation
{
  

    public class CartItemDtoValidator : AbstractValidator<CartItemDto>
    {
        public CartItemDtoValidator()
        {
            RuleFor(i => i.ProductId).NotEmpty();
            RuleFor(i => i.Quantity).GreaterThan(0);
        }
    }

    public class CartDtoValidator : AbstractValidator<CartDto>
    {
        public CartDtoValidator()
        {
            RuleFor(c => c.UserId).NotEmpty();
            RuleForEach(c => c.Items).SetValidator(new CartItemDtoValidator());
        }
    }

}
