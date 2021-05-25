using Dapper;
using Discount.Grpc.Dtos;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        private const string GET_COUPONS_SQL = "SELECT * FROM Coupon";
        private const string GET_DISCOUNT_SQL = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
        private const string GET_DISCOUNT_BY_ID_SQL = "SELECT * FROM Coupon WHERE ProductId = @Id";
        private const string GET_DISCOUNT_BY_RECORDID_SQL = "SELECT * FROM Coupon WHERE Id = @Id";
        private const string CREATE_DISCOUNT_SQL = "INSERT INTO Coupon (ProductName, ProductId, Description, Amount) VALUES (@ProductName, @ProductId, @Description, @Amount) RETURNING Id";
        private const string UPDATE_DISCOUNT_SQL = "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id";
        private const string DELETE_DISCOUNT_SQL = "DELETE FROM Coupon WHERE ProductName = @ProductName";
        public DiscountRepository(IConfiguration configuration)
        {
            _config = configuration ?? throw new ArgumentNullException(nameof(configuration));
            var server = _config["POSTGRES_SERVER"];// ?? "localhost";
            var port = _config["POSTGRES_PORT"];// ?? "5432";
            var database = _config["POSTGRES_DB"];// ?? "hess_catalog_db";
            var user = _config["POSTGRES_USER"];// ?? "marcus";
            var password = _config["POSTGRES_PASSWORD"];// ?? "password";

            _connectionString = $"Host={server}; Port={port}; Database={database}; Username={user}; Password={password};";
            Console.WriteLine($"CONNECTION STRING Discount: {_connectionString}");

        }

        public async Task<bool> DiscountExists(int id)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                (GET_DISCOUNT_BY_RECORDID_SQL, new { Id = id });
            return (coupon != null);
        }

        public async Task<IEnumerable<Coupon>> GetDiscounts()
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                var coupons = await connection.QueryAsync<Coupon>(GET_COUPONS_SQL);
                return coupons;
            }
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                (GET_DISCOUNT_SQL, new { ProductName = productName });

            return coupon;//!= null ? coupon : new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };

        }
        public async Task<Coupon> GetDiscountById(string productId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                (GET_DISCOUNT_BY_ID_SQL, new { Id = productId });

            return coupon;
        }

        async Task<Coupon> GetDiscountById(int recordId)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var coupon = await connection.QueryFirstOrDefaultAsync<Coupon>
                (GET_DISCOUNT_BY_RECORDID_SQL, new { Id = recordId });

            return coupon;
        }



        public async Task<Coupon> CreateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var newCouponId =
                await connection.ExecuteScalarAsync<int>
                    (CREATE_DISCOUNT_SQL,
                            new { ProductName = coupon.ProductName, ProductId = coupon.ProductId, Description = coupon.Description, Amount = coupon.Amount });

            return await GetDiscountById(newCouponId);
        }

        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync
                    (UPDATE_DISCOUNT_SQL,
                            new { ProductName = coupon.ProductName, Description = coupon.Description, Amount = coupon.Amount, Id = coupon.Id });

            return affected != 0;
        }

        public async Task<bool> DeleteDiscount(string productName)
        {
            using var connection = new NpgsqlConnection(_connectionString);

            var affected = await connection.ExecuteAsync(DELETE_DISCOUNT_SQL,
                new { ProductName = productName });

            return affected != 0;
        }
    }
}
