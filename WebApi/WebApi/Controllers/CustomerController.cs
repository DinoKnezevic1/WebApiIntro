﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
                command.CommandText = "select * from \"Customer\"";
                command.Connection = connection;
                connection.Open();

                NpgsqlDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "No elements exist");
                }
                while (reader.Read())
                {
                    Customer customer = new Customer();

                    customer.Id=(Guid)reader["Id"];
                    customer.FirstName = (string)reader["FirstName"];
                    customer.LastName = (string)reader["LastName"];

                    customers.Add(customer);
                }
                return Request.CreateResponse(HttpStatusCode.OK,customers);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(Guid id)
        {
            
            try
            {
                Customer customer = GetCustomerById(id);

                if (customer == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound, "Customer does not exist!");
                }
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            }catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody] Customer customer)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            
            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "INSERT INTO customer values(@Id,@FirstName,@LastName)";
                connection.CreateCommand();
                connection.Open();

                Guid id = Guid.NewGuid();
                customer.Id = id;
                command.Parameters.AddWithValue("@Id", customer.Id);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);

                command.ExecuteNonQuery();
            }


            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [HttpPut]
        public HttpResponseMessage Put(Guid id, [FromBody]Customer customer)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            Customer _customer = GetCustomerById(id);

            if (_customer == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "This user does not exist");
            }
            try
            {
                using (connection)
                {
                    var queryBuilder = new StringBuilder("");
                    NpgsqlCommand command = new NpgsqlCommand();
                    queryBuilder.Append("UPDATE Customer SET ");
                    //command.CommandText = "UPDATE customer SET \"firstname\" = @firstName, \"lastname\" = @lastName WHERE \"id\" = @id";
                    command.Connection = connection;
                    connection.Open();

                    if (customer.FirstName == null || customer.FirstName == "")
                    {   
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName = _customer.FirstName);
                    }
                    queryBuilder.Append(" \"Firstname\" = @FirstName,");
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    if (customer.LastName == null || customer.LastName == "")
                    {
                        command.Parameters.AddWithValue("@LastName", customer.LastName = _customer.LastName);
                    }
                    queryBuilder.Append(" \"Lastname\" = @LastName,");
                    command.Parameters.AddWithValue("@LastName", customer.LastName);

                    if (queryBuilder.ToString().EndsWith(","))
                    {
                        if(queryBuilder.Length > 0)
                        {
                            queryBuilder.Remove(queryBuilder.Length-1,1);
                        }
                    }

                    queryBuilder.Append(" WHERE \"Id\" = @Id");
                    command.Parameters.AddWithValue("@Id", id);
                    command.CommandText = queryBuilder.ToString();
                    command.ExecuteNonQuery();
                    return Request.CreateResponse(HttpStatusCode.OK, "User updated successfuly!");
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,ex.Message);
            }

        }

        [HttpDelete]
        public HttpResponseMessage Delete(Guid id)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            try
            {
                Customer customer = GetCustomerById(id);

                if (customer == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "This user does not exist");
                }
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.CommandText = "DELETE FROM customer WHERE \"Id\"=@Id";
                    command.Connection = connection;
                    connection.Open();

                    command.Parameters.AddWithValue("@Id",id);
                    command.ExecuteNonQuery();
                    return Request.CreateResponse(HttpStatusCode.OK, "User deleted successfuly!");
                }
            }catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Couldn't delete!"+ex.Message);
            }
        }

        private Customer GetCustomerById(Guid id)
        {
            string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
            NpgsqlConnection connection = new NpgsqlConnection(connectionString);

            using (connection)
            {
                NpgsqlCommand command = new NpgsqlCommand();
                command.CommandText = "select * from \"Customer\" where \"Id\"=@Id";
                command.Connection = connection;
                command.Parameters.AddWithValue("@Id",id);
                connection.Open();

                NpgsqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Customer customer = new Customer();
                    customer.Id = id;
                    customer.FirstName = (string)reader["FirstName"];
                    customer.LastName = (string)reader["LastName"];
                    return customer;
                }
                return null;
            }
        }
    }
}