using Application.Order.Commands;
using Application.Srock.Commands.RedeceStock;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using MediatR;
namespace UnitTests.Application.Orders
{
    public class ConfirmShippingCommandTests
    {
        private readonly Mock<IApplicationDbContext> _contextMock = new();
        private readonly Mock<IMediator> _mediatorMock = new();
        private readonly Mock<INotificationService> _notificationServiceMock = new();

        [Fact]
        public async Task Handle_ValidOrder_ReducesStockAndMarksAsShipped()
        {
            // Arrange
            var order = new Order
            {
                Id = 1,
                IsShipped = false,
                OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductOptionId = 1, Quantity = 2 }
            }
            };

            var orderData = new List<Order> { order }.AsQueryable();

            var mockDbSet = new Mock<DbSet<Order>>();
            mockDbSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(orderData.Provider);
            mockDbSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(orderData.Expression);
            mockDbSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(orderData.ElementType);
            mockDbSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(orderData.GetEnumerator());

            _contextMock.Setup(x => x.Orders).Returns(mockDbSet.Object);
            _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            _mediatorMock.Setup(x => x.Send(It.IsAny<ReduceStockCommand>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var handler = new ConfirmShippingCommandHandler(
                _contextMock.Object,
                _mediatorMock.Object,
                _notificationServiceMock.Object
            );

            var command = new ConfirmShippingCommand { OrderId = 1 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            Assert.True(order.IsShipped);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(x => x.Send(It.IsAny<ReduceStockCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _notificationServiceMock.Verify(x => x.SendNotificationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_OrderNotFound_ReturnsFalse()
        {
            // Arrange
            var emptyOrderData = new List<Order>().AsQueryable();

            var emptyMockDbSet = new Mock<DbSet<Order>>();
            emptyMockDbSet.As<IQueryable<Order>>().Setup(m => m.Provider).Returns(emptyOrderData.Provider);
            emptyMockDbSet.As<IQueryable<Order>>().Setup(m => m.Expression).Returns(emptyOrderData.Expression);
            emptyMockDbSet.As<IQueryable<Order>>().Setup(m => m.ElementType).Returns(emptyOrderData.ElementType);
            emptyMockDbSet.As<IQueryable<Order>>().Setup(m => m.GetEnumerator()).Returns(emptyOrderData.GetEnumerator());

            _contextMock.Setup(x => x.Orders).Returns(emptyMockDbSet.Object);

            var handler = new ConfirmShippingCommandHandler(
                _contextMock.Object,
                _mediatorMock.Object,
                _notificationServiceMock.Object
            );

            var command = new ConfirmShippingCommand { OrderId = 99 };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }

}
