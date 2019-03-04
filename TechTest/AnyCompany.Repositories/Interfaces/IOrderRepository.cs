using AnyCompany.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        bool Save(Order order);
        IEnumerable<Order> GetCustomerOrders(int customerId);
    }
}
