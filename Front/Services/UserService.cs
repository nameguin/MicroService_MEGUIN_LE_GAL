﻿using Front.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;

namespace Front.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private ProtectedLocalStorage _sessionStorage;

        public UserService(HttpClient httpClient, ProtectedLocalStorage sessionStorage)
        {
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
        }

        public async Task<UserDTO[]?> GetAllUsers()
        {
            try
            {
                var token = await _sessionStorage.GetAsync<string>("jwt");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5000/api/User");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserDTO[]>();

                    return result ?? Array.Empty<UserDTO>();
                }
                else
                {
                    return Array.Empty<UserDTO>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllUsers: {ex.Message}");
                return Array.Empty<UserDTO>();
            }
        }

        public async Task<UserDTO?> GetMyUserDTO()
        {
            try
            {
                var token = await _sessionStorage.GetAsync<string>("jwt");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5000/api/User");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UserDTO>();

                    return result;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUser: {ex.Message}");
                return null;
            }
        }

        public async Task<string> UpdateUser(UserUpdateModel userNewInfo)
        {
            var token = await _sessionStorage.GetAsync<string>("jwt");

            if (token.Success)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

                var userUpdate = new UserUpdateModel() { Id = userNewInfo.Id, Name = userNewInfo.Name, Email = userNewInfo.Email, Password = userNewInfo.Password };

                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"http://localhost:5000/api/User/{userNewInfo.Id}", userUpdate);

                Console.WriteLine(response.Content.ToString());
                Console.WriteLine(response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    return "";
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return error;
                }
            }
            Console.WriteLine("Erreur : Token JWT impossible à récupérer");
            return "Invalid Token";
        }

    }
}
