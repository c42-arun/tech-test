using System;

namespace AnyCompany.Repositories.Entities
{
    public class Customer
    {
        // new 'Id' property
        public int Id { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Name { get; set; }
    }
}
