using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Constants
{
    public abstract class Roles
    {
        public const string Administrator = nameof(Administrator);
        public const string SalesManager = nameof(SalesManager);
        public const string InventoryManager =nameof(InventoryManager);
        public const string Client = nameof(Client);
    }
}
