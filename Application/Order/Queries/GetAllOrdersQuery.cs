using Application.DTOs.Order;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Queries
{
    public class GetAllOrdersQuery : IRequest<List<OrderDto>>
    {
    }
}
