namespace AnyCompany.Repositories.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public double VAT { get; set; }

        // new "foreign key" on Customers.Customer table's PK - possibly enforced by triggers
        public int CustomerId { get; set; }
    }
}
