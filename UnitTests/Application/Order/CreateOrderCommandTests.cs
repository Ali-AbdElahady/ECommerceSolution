using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Order;
using Application.DTOs.Stock;
using Application.Orders.Commands.CreateOrder;
using Application.Srock.Commands.ReserveStock;
using Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace UnitTests.Application.Orders
{
    public class CreateOrderCommandTests
    {
        [Fact]
        public async Task Handle_Should_CreateOrder_And_ReserveStock()
        {
            // Arrange
            var contextMock = new Mock<IApplicationDbContext>();
            var mediatorMock = new Mock<IMediator>();
            var notificationServiceMock = new Mock<INotificationService>();

            var productOption = new ProductOption { Id = 1, ProductId = 1, Price = 100 };
            var orderDbSetMock = DbContextMock.GetQueryableMockDbSet(new List<Order>());
            var optionDbSetMock = DbContextMock.GetQueryableMockDbSet(new List<ProductOption> { productOption });
            var identityServiceMock = new Mock<IIdentityService>();

            contextMock.Setup(c => c.ProductOptions).Returns(optionDbSetMock.Object);
            contextMock.Setup(c => c.Orders).Returns(orderDbSetMock.Object);

            mediatorMock.Setup(m => m.Send(It.IsAny<ReserveStockCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(true);

            var handler = new CreateOrderCommandHandler(contextMock.Object, mediatorMock.Object, notificationServiceMock.Object, identityServiceMock.Object);

            var command = new CreateOrderCommand
            {
                CustomerId = "customer-123",
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto { ProductId = 1, OptionId = 1, Quantity = 2 }
                }
            };

            // Act
            var orderId = await handler.Handle(command, CancellationToken.None);

            // Assert
            orderId.Should().BeGreaterThan(0);
            contextMock.Verify(c => c.Orders.Add(It.IsAny<Order>()), Times.Once);
            contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.IsAny<ReserveStockCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            notificationServiceMock.Verify(n => n.SendNotificationAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }

    // Helper to mock DbSet<T>
    public static class DbContextMock
    {
        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
            return dbSet;
        }
    }
}
