using Example.Common;
using Example.Model;
using Example.Repository;
using Example.Repository.Common;
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
        private ICustomerRepository _customerRepository;
        
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<PagedList<Customer>> GetCustomersAsync(Sorting sorting, Paging paging, Filtering filtering)
        {
            return await _customerRepository.GetCustomersAsync(sorting, paging,  filtering);
        }

        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            return await _customerRepository.GetCustomerAsync(id);
        }

        public async Task<bool> SaveCustomerAsync(Customer customer)
        {
            return await _customerRepository.SaveCustomerAsync(customer);
        }

        public async Task<bool> UpdateCustomerAsync(Guid id, Customer customer)
        {
            return await _customerRepository.UpdateCustomerAsync(id, customer);
        }
        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            return await _customerRepository.DeleteCustomerAsync(id);
        }
    }
}
