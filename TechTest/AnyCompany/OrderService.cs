using System.Collections.Generic;
using System.Linq;
using AutoMapper;

using AnyCompany.Repositories.Entities;
using AnyCompany.Repositories.Interfaces;
using AnyCompany.ViewModels;

namespace AnyCompany
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;

            ConfigureAutoMapper();
        }

        public bool PlaceOrder(OrderViewModel order, int customerId)
        {
            Order orderEntity = _mapper.Map<Order>(order);

            Customer customer = _customerRepository.GetCustomer(customerId);

            if (order.Amount == 0)
                return false;

            // Saving customer id to new field in Orders table
            order.CustomerId = customerId;

            // TODO: this could be factored out to a service
            if (customer.Country == "UK")
                order.VAT = 0.2d;
            else
                order.VAT = 0;

            _orderRepository.Save(orderEntity);

            return true;
        }

        // This could be better written as a stored procedure pulls in 'Orders.Order' rows from remote linked server for each Customer record
        public IEnumerable<CustomerViewModel> GetCustomersWithOrders()
        {
            IEnumerable<Customer> customerEntities = _customerRepository.GetCustomers().ToList();
            IEnumerable<CustomerViewModel> customers = _mapper.Map<IEnumerable<Customer>, IEnumerable <CustomerViewModel>>(customerEntities);

            foreach (CustomerViewModel customer in customers)
            {
                List<Order> orderEntities = _orderRepository.GetCustomerOrders(customer.Id).ToList();
                IEnumerable<OrderViewModel> orders = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderViewModel>>(orderEntities);


                customer.Orders = orders.ToArray();
            }

            return customers;
        }

        // this should be performed once during system startup
        private void ConfigureAutoMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderViewModel, Order>();
                cfg.CreateMap<Customer, CustomerViewModel>();
                cfg.CreateMap<Order, OrderViewModel>();
            });

            _mapper = config.CreateMapper();
        }
    }
}
