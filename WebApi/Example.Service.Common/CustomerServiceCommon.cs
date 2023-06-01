using Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Service.Common
{
    public interface ICustomerService
    {
        Customer GetCustomer(Guid id);

        List<Customer> GetCustomers();

        bool SaveCustomer(Customer customer);

        bool DeleteCustomer(Guid id);
        bool UpdateCustomer(Guid id, Customer customer);
    }
}
