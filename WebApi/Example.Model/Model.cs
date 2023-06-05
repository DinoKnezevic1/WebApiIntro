using Example.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Model
{
    public class Customer : ICustomer
    {
        public Guid? Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class CustomersResponse
    {
        public List<Customer> Customers { get; set; }
        public int TotalCustomers { get; set; }
    }
}
