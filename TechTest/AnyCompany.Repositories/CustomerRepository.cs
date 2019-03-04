using System;
using System.Collections.Generic;
using System.Data.SqlClient;

using AnyCompany.Repositories.Entities;

namespace AnyCompany.Repositories
{
    // Would be cleaner to introduce an ORM such as Enyity Framework
    internal static class CustomerRepository
    {
        // TODO: move conenction string to configuration file/service
        private static string ConnectionString = @"Data Source=(local);Database=Customers;User Id=admin;Password=password;";

        public static Customer Load(int customerId)
        {
            Customer customer = new Customer();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand("SELECT TOP 1 Id, Name, DateOfBirth, Country FROM Customer WHERE CustomerId = @customerId",
                    connection);
                SqlParameter customerIdParam = new SqlParameter("@customerId", System.Data.SqlDbType.Int);
                customerIdParam.Value = customerId;

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    customer.Id = int.Parse(reader["Id"].ToString());
                    customer.Name = reader["Name"].ToString();
                    customer.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                    customer.Country = reader["Country"].ToString();
                }
            }

            return customer;
        }

        public static IEnumerable<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand customerCommand = new SqlCommand("SELECT Id, Name, DateOfBirth, Country FROM Customer", connection);

                var reader = customerCommand.ExecuteReader();

                while (reader.Read())
                {
                    Customer customer = new Customer();

                    customer.Id = int.Parse(reader["Id"].ToString());
                    customer.Name = reader["Name"].ToString();
                    customer.DateOfBirth = DateTime.Parse(reader["DateOfBirth"].ToString());
                    customer.Country = reader["Country"].ToString();


                    customers.Add(customer);
                }
            }

            return customers;
        }
    }
}
