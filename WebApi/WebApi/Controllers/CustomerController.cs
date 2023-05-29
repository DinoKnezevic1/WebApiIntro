using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        [HttpGet]
        public HttpResponseMessage Get()
        {
            List<Customer> customers = new List<Customer>();

            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "select * from \"customer\"";
                command.Connection = connection;
                connection.Open();

                NpgsqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, "No elements exist"); 
                }
                while (reader.Read())
                {
                    Customer customer = new Customer();

                    customer.Id=(Guid)reader["id"];
                    customer.FirstName = (string)reader["firstName"];
                    customer.LastName = (string)reader["lastName"];

                    customers.Add(customer);
                }
                return Request.CreateResponse(System.Net.HttpStatusCode.OK,customers);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            Customer customer = GetCustomerById(id);

            if(customer == null)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Customer does not exist!");
            }
            try
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.OK, customer);
            }catch (Exception ex)
            {
                return Request.CreateResponse(System.Net.HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] Customer customer)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            Customer _customer = null;
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "INSERT INTO customer values(@id,@firstName,@lastName)";
                connection.CreateCommand();
                connection.Open();

                Guid id = Guid.NewGuid();
                customer.Id = id;
                command.Parameters.AddWithValue("@id", customer.Id);
                command.Parameters.AddWithValue("@firstName", customer.FirstName);
                command.Parameters.AddWithValue("@lastName", customer.LastName);

                command.ExecuteNonQuery();
            }


            return Request.CreateResponse(System.Net.HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage Put(Guid id, [FromBody]Customer customer)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            Customer _customer = GetCustomerById(id);

            if (_customer == null)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "This user does not exist");
            }
            try
            {
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.CommandText = "UPDATE customer SET \"firstname\" = @firstName, \"lastname\" = @lastName WHERE \"id\" = @id";
                    command.Connection = connection;
                    connection.Open();

                    command.Parameters.AddWithValue("@id", id);
                    if (customer.FirstName == null || customer.FirstName.Length == 0)
                    {
                        command.Parameters.AddWithValue("@firstname", customer.FirstName = _customer.FirstName);
                    }
                    command.Parameters.AddWithValue("@firstName", customer.FirstName);
                    if (customer.LastName == null || customer.LastName.Length == 0)
                    {
                        command.Parameters.AddWithValue("@lastName", customer.LastName = _customer.LastName);
                    }
                    command.Parameters.AddWithValue("@lastName", customer.LastName);

                    command.ExecuteNonQuery();
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, "User updated successfuly!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest,ex.Message);
            }

        }

        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            Customer customer = GetCustomerById(id);

            if (customer == null)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "This user does not exist");
            }
            try
            {
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.CommandText = "DELETE FROM customer WHERE \"id\"=@id";
                    command.Connection = connection;
                    connection.Open();

                    command.Parameters.AddWithValue("@id",id);
                    command.ExecuteNonQuery();
                    return Request.CreateResponse(System.Net.HttpStatusCode.OK, "User deleted successfuly!");
                }
            }catch (Exception ex)
            {
                return Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Couldn't delete!"+ex.Message);
            }
        }

        private Customer GetCustomerById(Guid id)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "select * from \"customer\" where \"id\"=@id";
                command.Connection = connection;
                command.Parameters.AddWithValue("@id",id);
                connection.Open();

                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Customer customer = new Customer();
                    customer.Id = id;
                    customer.FirstName = (string)reader["firstName"];
                    customer.LastName = (string)reader["lastName"];
                    return customer;
                }
                return null;
            }
        }
    }
}