using Example.Common;
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

        /*
        [HttpGet]
        public async Task<List<Customer>> GetCustomersAsync()
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
                    await connection.OpenAsync();

                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
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
        */

        [HttpGet]
        public async Task<List<Customer>> GetCustomersAsync([FromUri] Sorting sorting, [FromUri] Paging paging, [FromUri] Filtering filtering)
        {
            List<Customer> customers = new List<Customer>();

            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                using (connection)
                {
                    var queryBuilder = new StringBuilder();
                    NpgsqlCommand command = new NpgsqlCommand();

                    NpgsqlCommand commandTotal = new NpgsqlCommand();
                    commandTotal.CommandText = "SELECT COUNT(\"FirstName\") FROM \"Customer\"";
                    command.Connection = connection;
                    
                    queryBuilder.Append("SELECT * FROM \"Customer\" ");

                    queryBuilder.Append("WHERE 1 = 1 ");
                    queryBuilder.Append(FilterQuery(filtering,command));

                    if (sorting.OrderBy != null || sorting.OrderBy.Length != 0)
                    {
                        queryBuilder.Append("ORDER BY \"Customer\".");
                        queryBuilder.Append("\""+sorting.OrderBy+"\"" + " ");
                    }
                    else
                    {
                        queryBuilder.Append("ORDER BY \"FirstName\" ");
                    }
                    if (sorting.SortOrder != null || sorting.SortOrder.Length != 0)
                    {
                        queryBuilder.Append(sorting.SortOrder + " ");
                    }
                    else
                    {
                        queryBuilder.Append("ASC ");
                    }
                    queryBuilder.Append("LIMIT " + paging.ItemsPerPage + " ");
                    queryBuilder.Append("OFFSET " + paging.PageNumber + " ");
                    command.CommandText = queryBuilder.ToString();
                    connection.CreateCommand();
                    await connection.OpenAsync();

                
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
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
            private StringBuilder FilterQuery(Filtering filtering,NpgsqlCommand command)
            {
                StringBuilder queryBuilder = new StringBuilder();
                if (filtering.SearchQuery != null)
                {
                    queryBuilder.Append("AND \"FirstName\" = @FirstName ");
                    command.Parameters.AddWithValue("FirstName", filtering.SearchQuery);
                }
                if (filtering.StartingLetter != null)
                {
                    queryBuilder.Append("AND \"FirstName\" LIKE ");
                    queryBuilder.Append("'" + filtering.StartingLetter + "%" + "' ");
                }
                return queryBuilder;
            }
        

        [HttpGet]
        public async Task<Customer> GetCustomerAsync(Guid id)
        {
            try
            {
                Customer customer = await GetCustomerByIdAsync(id);

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
        public async Task<bool> SaveCustomerAsync([FromBody]Customer customer)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.CommandText = "INSERT INTO \"Customer\" values(@Id,@FirstName,@LastName)";
                    command.Connection = connection;
                    connection.CreateCommand();
                    await connection.OpenAsync();

                    Guid id = Guid.NewGuid();
                    customer.Id = id;
                    command.Parameters.AddWithValue("@Id", customer.Id);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);

                    await command.ExecuteNonQueryAsync();
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
        public async Task<bool> UpdateCustomerAsync(Guid id, [FromBody]Customer customer)
        {
            Customer currentCustomer = await GetCustomerByIdAsync(id);

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
                    queryBuilder.Append("UPDATE \"Customer\" SET ");
                    command.Connection = connection;
                    await connection.OpenAsync();

                    if (customer.FirstName!=null || customer.FirstName.Length!=0)
                    {
                        queryBuilder.Append(" \"FirstName\" = @FirstName,");
                        command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    }
                    if (customer.LastName != null || customer.LastName.Length!=0)
                    {
                        queryBuilder.Append(" \"LastName\" = @LastName,");
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
                    await command.ExecuteNonQueryAsync();
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
        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            try
            {
                NpgsqlConnection connection = new NpgsqlConnection(connectionString);
                Customer customer = await GetCustomerByIdAsync(id);

                if (customer == null)
                {
                    return false;
                }
                using (connection)
                {
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.CommandText = "DELETE FROM \"Customer\" WHERE \"Id\"=@Id";
                    command.Connection = connection;
                    await connection.OpenAsync();

                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        private async Task<Customer> GetCustomerByIdAsync(Guid id)
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
                    await connection.OpenAsync();

                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
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
