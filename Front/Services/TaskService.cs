using Front.Entities;
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
    public class TaskService
    {
        private readonly HttpClient _httpClient;
        private ProtectedLocalStorage _sessionStorage;

        public TaskService(HttpClient httpClient, ProtectedLocalStorage sessionStorage)
        {
            _httpClient = httpClient;
            _sessionStorage = sessionStorage;
        }

        public async Task<TaskModel[]> GetAllTasks()
        {
            try
            {
                 var token = await _sessionStorage.GetAsync<string>("jwt");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

                HttpResponseMessage response = await _httpClient.GetAsync("http://localhost:5000/api/Task");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TaskModel[]>();

                    return result ?? Array.Empty<TaskModel>();
                }
                else
                {
                    return Array.Empty<TaskModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllTasks: {ex.Message}");
                return Array.Empty<TaskModel>();
            }
        }

        public async Task<TaskModel?> CreateTask()
        {
            var token = await _sessionStorage.GetAsync<string>("jwt");

            if (token.Success)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

                var task = new TaskCreateModel() { IsDone = false, Text = "Empty" };
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/Task/create", task);

                Console.WriteLine(response.Content.ToString());
                Console.WriteLine(response.StatusCode);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TaskModel>();

                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Erreur : Token JWT impossible à récupérer");
                return null;
            }
        }

        public async Task<TaskModel?> UpdateTask(TaskModel todo)
        {

            var token = await _sessionStorage.GetAsync<string>("jwt");

            if (token.Success)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

                var task = new TaskCreateModel() { IsDone = todo.IsDone, Text = todo.Text };

                Console.WriteLine($"update todo {todo.Id} {todo.IsDone} {todo.Text}");
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"http://localhost:5000/api/Task/update/{todo.Id}", task);

                Console.WriteLine(response.Content.ToString());
                Console.WriteLine(response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<TaskModel>();

                    return result;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Erreur : Token JWT impossible à récupérer");
                return null;
            }
        }

        public async Task DeleteTask(int id)
        {

            var token = await _sessionStorage.GetAsync<string>("jwt");

            if (token.Success)
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);

                HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5000/api/Task/delete/{id}");

                Console.WriteLine(response.Content.ToString());
                Console.WriteLine(response.StatusCode);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Error deleting");
                }
            }
            else
            {
                Console.WriteLine("Erreur : Token JWT impossible à récupérer");
                throw new Exception("Error deleting");
            }
        }

    }
}