﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Order.Commands
{
    public class ConfirmShippingCommand : IRequest<bool>
    {
        public int OrderId { get; set; }
    }
}
