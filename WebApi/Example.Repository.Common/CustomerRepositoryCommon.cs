﻿using Example.Common;
using Example.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example.Repository.Common
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerAsync(Guid id);

        Task<PagedList<Customer>> GetCustomersAsync(Sorting sorting, Paging paging, Filtering filtering);

        Task<bool> SaveCustomerAsync(Customer customer);

        Task<bool> DeleteCustomerAsync(Guid id);
        Task<bool> UpdateCustomerAsync(Guid id, Customer customer);
    }
}
