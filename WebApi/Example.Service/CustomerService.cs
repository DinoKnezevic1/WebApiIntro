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
        public List<Customer> GetCustomers()
        {
            CustomerRepository customerRepository = new CustomerRepository();
            return customerRepository.GetCustomers();
        }

        public Customer GetCustomer(Guid id)
        {
            CustomerRepository customersRepository = new CustomerRepository();
            return customersRepository.GetCustomer(id);
        }

        public bool SaveCustomer(Customer customer)
        {
            CustomerRepository customerRepository = new CustomerRepository();
            return customerRepository.SaveCustomer(customer);
        }

        public bool UpdateCustomer(Guid id, Customer customer)
        {
            CustomerRepository customerRepository=new CustomerRepository();
            return customerRepository.UpdateCustomer(id, customer);
        }
        public bool DeleteCustomer(Guid id)
        {
            CustomerRepository customerRepository = new CustomerRepository();
            return customerRepository.DeleteCustomer(id);
        }
    }
}
