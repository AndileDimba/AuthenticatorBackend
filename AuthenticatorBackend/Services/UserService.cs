using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthenticatorBackend.Services
{
    public class UserService
    {
        private readonly string _connectionString;
        private readonly ILogger<UserController> _logger;

        public UserService(IConfiguration configuration, ILogger<UserController> logger)
            {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> RegisterUser(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Check if the username already exists
                using (var checkCommand = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Username = @Username", connection))
                {
                    checkCommand.Parameters.AddWithValue("@Username", username);
                    var userExists = (int)await checkCommand.ExecuteScalarAsync() > 0;
                    if (userExists)
                    {
                        // Log that the user already exists
                        _logger.LogWarning(username," Already exists");
                        return false; // Indicate that the user already exists
                    }
                }

                using (var command = new SqlCommand("RegisterUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return true;
                    }
                    catch (SqlException ex)
                    {
                        // Log the detailed error
                        Console.WriteLine($"SQL Error Number {ex.Number}: {ex.Message}"); // For immediate debugging
                        return false;
                    }
                }
            }
        }

        public async Task<(bool IsValid, string Message)> LoginUser(string username, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("LoginUser", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    var result = await command.ExecuteScalarAsync();
                    return (result != null && (int)result == 1, result != null ? "Login successful." : "Invalid username or password.");
                }
            }
        }

        public async Task<bool> ResetPassword(string username, string newPassword)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("ResetPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@NewPassword", newPassword);

                    try
                    {
                        await command.ExecuteNonQueryAsync();
                        return true; // Password reset successful
                    }
                    catch (SqlException)
                    {
                        // Log exception or handle it as needed
                        return false; // Password reset failed
                    }
                }
            }
        }
    }
}