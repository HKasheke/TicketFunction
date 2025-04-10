using System;
using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TicketFunction
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Function1))]
        public async Task Run([QueueTrigger("tickethub", Connection = "AzureWebJobsStorage")] QueueMessage message)
        {
            _logger.LogInformation($"C# Queue trigger function processed: {message.MessageText}");

            string messageJson = message.MessageText;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserialize the message into a purchase object
            var purchase = JsonSerializer.Deserialize<Purchase>(message.MessageText, options);

            if (purchase == null)
            {
                _logger.LogError("Failed to deserialize message");
                return;
            }

            _logger.LogInformation($"Purchase: {purchase}");

            //add purchase to DB
            string? connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("SQL connection string is not set in the environement variable");
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                await conn.OpenAsync(); // Note the ASYNC
                var query = "INSERT INTO dbo.purchases (concertId, email, Name, phone, quantity, creditCard, expiration, securityCode, address, city, province, postalCode, country) VALUES (@concertId, @email, @Name, @phone, @quantity, @creditCard, @expiration, @securityCode, @address, @city, @province, @postalCode, @country)";
                
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@concertId", purchase.ConcertId);
                    cmd.Parameters.AddWithValue("@email", purchase.Email);
                    cmd.Parameters.AddWithValue("@name", purchase.Name);
                    cmd.Parameters.AddWithValue("@phone", purchase.Phone);
                    cmd.Parameters.AddWithValue("@quantity", purchase.Quantity);
                    cmd.Parameters.AddWithValue("@creditCard", purchase.CreditCard);
                    cmd.Parameters.AddWithValue("@expiration", purchase.Expiration);
                    cmd.Parameters.AddWithValue("@securityCode", purchase.SecurityCode);
                    cmd.Parameters.AddWithValue("@address", purchase.Address);
                    cmd.Parameters.AddWithValue("@city", purchase.City);
                    cmd.Parameters.AddWithValue("@province", purchase.Province);
                    cmd.Parameters.AddWithValue("@postalCode", purchase.PostalCode);
                    cmd.Parameters.AddWithValue("@country", purchase.Country);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
