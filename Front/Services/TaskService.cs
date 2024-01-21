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
                 var jwt = await _sessionStorage.GetAsync<string>("jwt");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);

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

        public async Task<(TaskModel? task, string? error)> CreateTask(string title, string? description, DateTime deadline)
        {
            var task = new TaskCreateModel() {
                IsDone = false,
                Title = title,
                Description = description,
                Deadline = deadline,

            };
            var jwt = await _sessionStorage.GetAsync<string>("jwt");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/Task/create", task);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TaskModel>();

                return (result,"");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (null, error);
            }
        }

        public async Task<(TaskModel? task, string? error)> UpdateTask(TaskModel todo)
        {
            var task = new TaskCreateModel() { IsDone = todo.IsDone, Title = todo.Title, Description = todo.Description, Deadline = todo.Deadline };

            var jwt = await _sessionStorage.GetAsync<string>("jwt");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"http://localhost:5000/api/Task/update/{todo.Id}", task);

            Console.WriteLine(response.Content.ToString());
            Console.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TaskModel>();

                return (result, "");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (null, error);
            }
        }

        public async Task DeleteTask(int id)
        {
            var jwt = await _sessionStorage.GetAsync<string>("jwt");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.Value);
            HttpResponseMessage response = await _httpClient.DeleteAsync($"http://localhost:5000/api/Task/delete/{id}");

            Console.WriteLine(response.Content.ToString());
            Console.WriteLine(response.StatusCode);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error deleting");
            }
        }

    }
}