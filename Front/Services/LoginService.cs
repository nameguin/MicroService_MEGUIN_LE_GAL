using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;

namespace Front.Services
{
    public class LoginService : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDTO?> AuthenticateUser(string username, string password)
        {
            UserLogin userlogin = new()
            {
                Name = username,
                Pass = password,
            };
            var response =  await _httpClient.PostAsJsonAsync("http://localhost:5000/api/User/login", userlogin);

            // Check if the response status code is 200 (OK)
            if (response.StatusCode == HttpStatusCode.OK)
            {
                // You can deserialize the response content here if needed
                var result = await response.Content.ReadFromJsonAsync<UserDTO>();
                return result;
            }
            return null;
        }
    }
}

