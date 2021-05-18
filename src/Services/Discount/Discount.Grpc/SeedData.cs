using System;
using System.Threading.Tasks;
using Npgsql;
using Microsoft.Extensions.Configuration;
namespace Discount.Grpc
{
    public class SeedData
    {
        public static async Task Initialize(IConfiguration config)
        {
            var server = config["POSTGRES_SERVER"];
            var port = config["POSTGRES_PORT"];
            var database = config["POSTGRES_DB"];
            var user = config["POSTGRES_USER"];
            var password = config["POSTGRES_PASSWORD"];
            var connectionString = $"Host={server}; Port={port};Username={user}; Password={password};";

            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        using var command = new NpgsqlCommand { Connection = connection };
                        command.CommandText = $"select count(*)  from pg_database where datname = '{database}';";


                        if (Convert.ToInt32(await command.ExecuteScalarAsync()) < 1)
                        {
                            await CreateDatabase(connection, database);
                        }
                        else
                        {
                            Console.WriteLine("Discount database already created");
                        }
                    }
                    else
                        throw new Exception($"Cant open connection when creating {database}");
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"Cant connect to {server}. Error is {ex.Message}");
                }

            }
            connectionString = connectionString + $" Database={database}; ";
            using (var connection = new NpgsqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        await PopulateTestData(connection);
                    }
                    else
                        throw new Exception($"Cant open connection when populating {database}");
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine($"Cant connect to {server}. Error is {ex.Message}");
                }

            }
        }

        private static async Task CreateDatabase(NpgsqlConnection connection, string databaseName)
        {
            using var command = new NpgsqlCommand { Connection = connection };
            command.CommandText = $"CREATE DATABASE {databaseName};";
            await command.ExecuteNonQueryAsync();
            Console.WriteLine("Created Discount database");
        }

        private static async Task PopulateTestData(NpgsqlConnection connection)
        {
            using var command = new NpgsqlCommand { Connection = connection };
            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            await command.ExecuteNonQueryAsync();

            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(80) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
            await command.ExecuteNonQueryAsync();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Hess 1990 Tanker Truck', 'Hess 1990 Discount', 10);";
            await command.ExecuteNonQueryAsync();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Hess 1996 Emergency Truck', 'Hess 1996 Emergency Truck Discount', 8);";
            await command.ExecuteNonQueryAsync();
            Console.WriteLine("Seeded Discount database");
        }
    }
}