using CreditReporting.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CreditReporting.Infrastructure.ExternalServices
{
    public class UserClient(HttpClient httpClient, IConfiguration configuration) : IUserClient
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly string _baseUrl = "http://localhost:5141";

        public async Task<UserDetailsDto?> GetCustomerByUserIdAsync(int userId)
        {
            try
            {
                // Logic to call User Service
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/v1/customers/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserApiResponse>();
                    return result?.Data;
                }
                return null;
            }
            catch (Exception)
            {
                // Log error
                return null;
            }
        }

        public async Task<UserDetailsDto?> GetCustomerByCustomerIdAsync(int customerId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_baseUrl}/api/customers/{customerId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserApiResponse>();
                    return result?.Data;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private class UserApiResponse
        {
            public bool Success { get; set; }
            public UserDetailsDto? Data { get; set; }
        }
    }
}
