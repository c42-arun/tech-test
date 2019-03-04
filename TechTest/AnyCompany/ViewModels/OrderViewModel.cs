using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public double Amount { get; set; }
        public double VAT { get; set; }

        // new "foreign key" on Customers.Customer table's PK - possibly enforced by triggers
        public int CustomerId { get; set; }
    }
}
