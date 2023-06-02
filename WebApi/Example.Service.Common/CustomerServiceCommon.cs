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
        Task<Customer> GetCustomerAsync(Guid id);

        Task<List<Customer>> GetCustomersAsync();

        Task<bool> SaveCustomerAsync(Customer customer);

        Task<bool> DeleteCustomerAsync(Guid id);
        Task<bool> UpdateCustomerAsync(Guid id, Customer customer);
    }
}
