using System;
using System.Threading.Tasks;
using Npgsql;

namespace Discount.Grpc
{
    public class SeedData
    {
        public static async Task Initialize(string connectionString)
        {

            using (var connection = new NpgsqlConnection(connectionString))
            {
                Console.WriteLine(connectionString);
                connection.Open();
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    await PopulateTestData(connection);

                }

            }
        }

        private static async Task PopulateTestData(NpgsqlConnection connection)
        {
            using var command = new NpgsqlCommand { Connection = connection };
            command.CommandText = "DROP TABLE IF EXISTS Coupon";
            await command.ExecuteNonQueryAsync();

            command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
            await command.ExecuteNonQueryAsync();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
            await command.ExecuteNonQueryAsync();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
            await command.ExecuteNonQueryAsync();
        }
    }
}