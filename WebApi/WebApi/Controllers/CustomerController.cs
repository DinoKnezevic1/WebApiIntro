using Example.Common;
using Example.Model;
using Example.Repository;
using Example.Service;
using Example.Service.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using WebApi.Models;
using HttpDeleteAttribute = System.Web.Http.HttpDeleteAttribute;
using HttpGetAttribute = System.Web.Http.HttpGetAttribute;
using HttpPostAttribute = System.Web.Http.HttpPostAttribute;
using HttpPutAttribute = System.Web.Http.HttpPutAttribute;

namespace WebApi.Controllers
{
    public class CustomerController : ApiController
    {

        private ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAsync(string OrderBy,string SortOrder, int ItemsPerPage, int PageNumber, string SearchQuery, string StartingLetter)
        {
            try
            {
                Sorting sorting = new Sorting();
                sorting.SortOrder = SortOrder;
                sorting.OrderBy= OrderBy;

                Paging paging = new Paging();
                paging.PageNumber = PageNumber;
                paging.ItemsPerPage = ItemsPerPage;

                Filtering filtering = new Filtering();
                filtering.StartingLetter = StartingLetter;
                filtering.SearchQuery = SearchQuery;

                List<Customer> customers = await _customerService.GetCustomersAsync(sorting,paging,filtering);
                if (customers.Any())
                {
                    List<CustomerRest> customersRest = MapCustomersToRest(customers);
                    return Request.CreateResponse(HttpStatusCode.OK, customers);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex);
            }
            
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAsync(Guid id)
        {
            try
            {
                Customer customer = await _customerService.GetCustomerAsync(id);

                if (customer == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer does not exist!");
                }
                return Request.CreateResponse(HttpStatusCode.OK, MapToRest(customer));
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] CustomerRest customer)
        {
            try
            {
                bool postStatus = await _customerService.SaveCustomerAsync(MapToCustomer(customer));
                if (postStatus)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, postStatus);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest, "User creation failed!");

            }catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> Put(Guid id, [FromBody]CustomerUpdateRest customer)
        {
            try
            {
                bool putStatus = await _customerService.UpdateCustomerAsync(id, MapToCustomerUpdate(customer));
                if (putStatus)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, putStatus);
                }
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,"Customer update failed");
            }catch(Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }

        }

        [HttpDelete]
        public async Task<HttpResponseMessage> Delete(Guid id)
        {
            try
            {
                bool deleteStatus = await _customerService.DeleteCustomerAsync(id);
                if (deleteStatus)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, deleteStatus);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }catch(Exception ex){
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex.Message);
            }
        }

        private CustomerRest MapToRest(Customer customer)
        {
            CustomerRest customerRest = new CustomerRest();
            customerRest.Id = customer.Id;
            customerRest.FirstName = customer.FirstName;
            customerRest.LastName = customer.LastName;
            return customerRest;
        }
        private List<CustomerRest> MapCustomersToRest(List<Customer> customers)
        {
            List<CustomerRest> customerRests = new List<CustomerRest>();

            foreach (Customer customer in customers)
            {
                customerRests.Add(MapToRest(customer));
            }
            return customerRests;
        }
        private Customer MapToCustomer(CustomerRest customerRest)
        {
            Customer customer = new Customer();
            customer.Id = customerRest.Id;
            customer.FirstName = customerRest.FirstName;
            customer.LastName = customerRest.LastName;
            return customer;
        }
        private Customer MapToCustomerUpdate(CustomerUpdateRest customerUpdate)
        {
            Customer customer = new Customer();
            customer.FirstName = customerUpdate.FirstName;
            customer.LastName = customerUpdate.LastName;
            return customer;
        }
    }
}