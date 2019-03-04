using System.Collections.Generic;
using System.Data.SqlClient;

using AnyCompany.Repositories.Entities;
using AnyCompany.Repositories.Interfaces;

namespace AnyCompany.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static string ConnectionString = @"Data Source=(local);Database=Orders;User Id=admin;Password=password;";

        public bool Save(Order order)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand("INSERT INTO Orders VALUES (@OrderId, @Amount, @VAT, @CustomerId)", connection);

                    command.Parameters.AddWithValue("@OrderId", order.Id);
                    command.Parameters.AddWithValue("@Amount", order.Amount);
                    command.Parameters.AddWithValue("@VAT", order.VAT);
                    command.Parameters.AddWithValue("@CustomerId", order.CustomerId);

                    command.ExecuteNonQuery();

                    return true;
                }
            }
            catch (System.Exception)
            {
                // handle error
                return false;
            }
        }

        public IEnumerable<Order> GetCustomerOrders(int customerId)
        {
            throw new System.NotImplementedException();
        }
    }
}
