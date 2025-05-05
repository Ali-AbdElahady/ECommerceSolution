using Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(CreateOrderDto orderDto);
        Task<List<OrderDto>> GetAllOrdersAsync();
        Task ConfirmOrderAsync(int orderId);
    }
}
