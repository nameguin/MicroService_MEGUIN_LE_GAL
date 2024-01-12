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
    public class TaskService
    {
        private readonly HttpClient _httpClient;

        public TaskService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(TaskModel? user, string error)> CreateTask(string text, bool done, DateTime date)
        {
            TaskModel taskModel = new()
            {
                Text = text,
                IsDone = done,
                Date = date,
            };

            var response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/Task/create", taskModel);

            // Check if the response status code is 200 (OK)
            if (response.IsSuccessStatusCode)
            {
                // You can deserialize the response content here if needed
                var result = await response.Content.ReadFromJsonAsync<TaskModel>();
                return (result, "");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                return (null, error);
            }
        }
    }
}