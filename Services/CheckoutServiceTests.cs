using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ProductionGrade.Abstractions;
using ProductionGrade.Models;
using ProductionGrade.Services;
using Xunit;

namespace ProductionGrade.Tests.Services
{
    public class CheckoutServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CheckoutService _checkoutService;

        public CheckoutServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _checkoutService = new CheckoutService(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task CheckoutAsync_ShouldReturnFalse_WhenCartIsEmpty()
        {
            // Arrange
            _unitOfWorkMock.Setup(u => u.Carts.GetByUserIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Cart?)null);

            // Act
            var result = await _checkoutService.CheckoutAsync(Guid.NewGuid());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task CheckoutAsync_ShouldReturnTrue_WhenCartHasItems()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var productId = Guid.NewGuid();

            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>
                {
                    new CartItem
                    {
                        ProductId = productId,
                        Quantity = 2,
                        Product = new Product
                        {
                            Id = productId,
                            Name = "Gaming Laptop",
                            Price = 1500,
                            StockQuantity = 10
                        }
                    }
                }
            };

            _unitOfWorkMock.Setup(u => u.Carts.GetByUserIdAsync(userId))
                .ReturnsAsync(cart);

            _unitOfWorkMock.Setup(u => u.Orders.CreateAsync(It.IsAny<Order>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.Carts.ClearCartAsync(userId))
                .ReturnsAsync(true);

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _checkoutService.CheckoutAsync(userId);

            // Assert
            Assert.True(result);
            _unitOfWorkMock.Verify(u => u.Orders.CreateAsync(It.IsAny<Order>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.Carts.ClearCartAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}
