using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using FluentAssertions;

using AnyCompany.Repositories.Interfaces;
using AnyCompany.ViewModels;
using AnyCompany.Repositories.Entities;

namespace AnyCompany.Tests
{
    public class OrderServiceTests
    {
        // might need to be "..LessThanOrEqualToZero.."?
        [Fact]
        [Trait("Order Creation", "Amount Check")]
        public void OrderWithZeroAmountShouldNotBeSaved()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();

            OrderService cut = new OrderService(mockOrderRepository.Object, mockCustomerRepository.Object);

            OrderViewModel order = new OrderViewModel { Amount = 0 };

            // Act
            bool returnValue = cut.PlaceOrder(order, 0);

            // Assert
            returnValue.Should().BeFalse("Order with 0 Amount should not be saved");
        }

        [Fact]
        [Trait("Order Creation", "Amount Check")]
        public void OrderWithPostiveAmountShouldBeSaved()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();

            mockCustomerRepository.Setup(_ => _.GetCustomer(It.IsAny<int>())).Returns(new Customer());

            OrderService cut = new OrderService(mockOrderRepository.Object, mockCustomerRepository.Object);

            OrderViewModel order = new OrderViewModel { Amount = 0.1 };

            // Act
            bool returnValue = cut.PlaceOrder(order, 0);

            // Assert
            returnValue.Should().BeTrue("PlaceOrder should return True when amount > 0");
        }

        [Fact]
        [Trait("Order Creation", "Customer Link")]
        public void OrderShouldBeSavedWithCustomer()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();

            mockCustomerRepository.Setup(_ => _.GetCustomer(It.IsAny<int>())).Returns(new Customer());

            OrderService cut = new OrderService(mockOrderRepository.Object, mockCustomerRepository.Object);

            OrderViewModel order = new OrderViewModel { Amount = 0.1 };

            // Act
            cut.PlaceOrder(order, customerId: 100);

            // Assert
            order.CustomerId.Should().Be(100, "Customer Id should be saved within an Order");
        }

        [Fact]
        [Trait("Order Creation", "Verify Save")]
        public void SavingValidOrderShouldCallSaveOnOrderRepository()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();

            mockCustomerRepository.Setup(_ => _.GetCustomer(It.IsAny<int>())).Returns(new Customer());

            OrderService cut = new OrderService(mockOrderRepository.Object, mockCustomerRepository.Object);

            OrderViewModel order = new OrderViewModel { Amount = 0.1 };

            // Act
            cut.PlaceOrder(order, 0);

            // Assert
            mockOrderRepository.Verify(_ => _.Save(It.IsAny<Order>()), "OrderRepository.Save() should be called for a valid order");
        }

        [Fact]
        [Trait("Order Creation", "VAT check")]
        public void VatShouldBeAddedForUkCustomer()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();

            mockCustomerRepository.Setup(_ => _.GetCustomer(It.IsAny<int>())).Returns(new Customer { Country = "UK" });

            OrderService cut = new OrderService(mockOrderRepository.Object, mockCustomerRepository.Object);

            OrderViewModel order = new OrderViewModel { Amount = 0.1 };

            // Act
            cut.PlaceOrder(order, 0);

            // Assert
            order.VAT.Should().Be(0.2);
        }

        [Theory]
        [InlineData("USA")]
        [InlineData("Germany")]
        [InlineData("Japan")]
        [Trait("Order Creation", "VAT check")]
        public void VatShouldBeZeroForNonUkCustomers(string country)
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();

            mockCustomerRepository.Setup(_ => _.GetCustomer(It.IsAny<int>())).Returns(new Customer { Country = country });

            OrderService cut = new OrderService(mockOrderRepository.Object, mockCustomerRepository.Object);

            OrderViewModel order = new OrderViewModel { Amount = 0.1 };

            // Act
            cut.PlaceOrder(order, 0);

            // Assert
            order.VAT.Should().Be(0);
        }

        [Fact]
        [Trait("Customers Retrieval", "Orders Link")]
        public void CustomersWithOrdersTest()
        {
            // Arrange
            var mockCustomerRepository = new Mock<ICustomerRepository>();
            var mockOrderRepository = new Mock<IOrderRepository>();

            mockCustomerRepository.Setup(_ => _.GetCustomers()).Returns(TestCustomers);
            mockOrderRepository.Setup(_ => _.GetCustomerOrders(It.Is<int>(x => x == 2))).Returns(TestOrders.Where(o => o.CustomerId == 2));

            OrderService cut = new OrderService(mockOrderRepository.Object, mockCustomerRepository.Object);

            // Act
            var customers = cut.GetCustomersWithOrders();

            // Assert
            var ordersForCustomer2 = customers.First(c => c.Id == 2).Orders.Count();

            ordersForCustomer2.Should().Be(TestOrders.Count(t => t.CustomerId == 2), "Orders for a given customer should be linked correctly");
        }

        #region test data

        private List<Customer> TestCustomers
        {
            get
            {
                return new List<Customer>
                {
                    new Customer { Id = 1 },
                    new Customer { Id = 2 },
                };
            }
        }

        private List<Order> TestOrders
        {
            get
            {
                return new List<Order>
                {
                    new Order { Id = 1, CustomerId = 1 },
                    new Order { Id = 2, CustomerId = 2 },
                    new Order { Id = 3, CustomerId = 2 }
                };
            }
        }

        #endregion test data
    }
}
