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
                                                                ProductName VARCHAR(80) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
            await command.ExecuteNonQueryAsync();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Hess 1990 Tanker Truck', 'Hess 1990 Discount', 10);";
            await command.ExecuteNonQueryAsync();

            command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Hess 1996 Emergency Truck', 'Hess 1996 Emergency Truck Discount', 8);";
            await command.ExecuteNonQueryAsync();
        }
    }
}