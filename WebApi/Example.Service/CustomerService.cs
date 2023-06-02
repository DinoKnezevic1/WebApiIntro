using Example.Model;
using Example.Repository;
using Example.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Example.Service
{
    public class CustomerService : ICustomerService
    {
        public async Task<List<Customer>> GetCustomersAsync()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            return await customerRepository.GetCustomersAsync();
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            CustomerRepository customersRepository = new CustomerRepository();
            return await customersRepository.GetCustomerAsync(id);
        }

        public async Task<bool> SaveCustomerAsync(Customer customer)
        {
            CustomerRepository customerRepository = new CustomerRepository();
            return await customerRepository.SaveCustomerAsync(customer);
        }

        public async Task<bool> UpdateCustomerAsync(Guid id, Customer customer)
        {
            CustomerRepository customerRepository=new CustomerRepository();
            return await customerRepository.UpdateCustomerAsync(id, customer);
        }
        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            CustomerRepository customerRepository = new CustomerRepository();
            return await customerRepository.DeleteCustomerAsync(id);
        }
    }
}
