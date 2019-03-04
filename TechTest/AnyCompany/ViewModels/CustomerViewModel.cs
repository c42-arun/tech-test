using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyCompany.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Name { get; set; }

        public ICollection<OrderViewModel> Orders { get; set; }
    }
}
