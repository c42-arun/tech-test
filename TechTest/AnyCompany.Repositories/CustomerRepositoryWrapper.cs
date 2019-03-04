using AnyCompany.Repositories.Entities;
using AnyCompany.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.Repositories
{
    public class CustomerRepositoryWrapper : ICustomerRepository
    {
        public Customer GetCustomer(int customerId)
        {
            return CustomerRepository.Load(customerId);
        }

        public IEnumerable<Customer> GetCustomers()
        {
            return CustomerRepository.GetAllCustomers();
        }
    }
}
