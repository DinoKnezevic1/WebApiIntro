using Example.Model;
using Example.Repository.Common;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Example.Repository
{
    public class CustomerRepository : ICustomerRepository
    {

        private string connectionString = "Host=localhost;Username=postgres;Password=mojabaza123;Database=rent-a-car";
        
        
        [HttpGet]
        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

            try
            {
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
                        return null;
                    }
                    while (reader.Read())
                    {
                        Customer customer = new Customer();

                        customer.Id = (Guid)reader["Id"];
                        customer.FirstName = (string)reader["FirstName"];
                        customer.LastName = (string)reader["LastName"];

                        customers.Add(customer);
                    }
                    return customers;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        [HttpGet]
        public Customer GetCustomer(Guid id)
        {
            try
            {
                Customer customer = GetCustomerById(id);

                if (customer == null)
                {
                    return null;
                }
                return customer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        [HttpPost]
        public bool SaveCustomer([FromBody]Customer customer)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.CommandText = "INSERT INTO customer values(@Id,@FirstName,@LastName)";
                    command.Connection = connection;
                    connection.CreateCommand();
                    connection.Open();

                    Guid id = Guid.NewGuid();
                    customer.Id = id;
                    command.Parameters.AddWithValue("@Id", customer.Id);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);

                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        [HttpPut]
        public bool UpdateCustomer(Guid id, [FromBody]Customer customer)
        {
            Customer currentCustomer = GetCustomerById(id);

            if (currentCustomer == null)
            {
                return false;
            }
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                using (connection)
                {
                    var queryBuilder = new StringBuilder();
                    NpgsqlCommand command = new NpgsqlCommand();
                    queryBuilder.Append("UPDATE Customer SET ");
                    command.Connection = connection;
                    connection.Open();

                    if (customer.FirstName!=null || customer.FirstName.Length!=0)
                    {
                        queryBuilder.Append(" \"Firstname\" = @FirstName,");
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    }

                    if (customer.LastName != null || customer.LastName.Length!=0)
                    {
                        queryBuilder.Append(" \"Lastname\" = @LastName,");
                        command.Parameters.AddWithValue("@LastName", customer.LastName);
                    }
                    if (queryBuilder.ToString().EndsWith(","))
                    {
                        if (queryBuilder.Length > 0)
                        {
                            queryBuilder.Remove(queryBuilder.Length - 1, 1);
                        }
                    }
                    queryBuilder.Append(" WHERE \"Id\" = @Id");
                    command.Parameters.AddWithValue("@Id", id);
                    command.CommandText = queryBuilder.ToString();
                    connection.CreateCommand();
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        [HttpDelete]
        public bool DeleteCustomer(Guid id)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                Customer customer = GetCustomerById(id);

                if (customer == null)
                {
                    return false;
                }
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.CommandText = "DELETE FROM customer WHERE \"Id\"=@Id";
                    command.Connection = connection;
                    connection.Open();

                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private Customer GetCustomerById(Guid id)
        {

            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.CommandText = "select * from \"Customer\" where \"Id\"=@Id";
                    command.Connection = connection;
                    command.Parameters.AddWithValue("@Id", id);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            
        }
    }
}
