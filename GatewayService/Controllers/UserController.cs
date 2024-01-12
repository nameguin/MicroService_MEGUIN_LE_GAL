using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using UserService.Entities;

namespace GatewayService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IHttpClientFactory _httpClientFactory;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // api/User/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLogin model)
        {
            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/login", model);

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode)
                {
                    // You can deserialize the response content here if needed
                    var result = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Login failed");
                }
            }
        }

        // api/User/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateModel model)
        {

            if (!model.Name.All(c => Char.IsLetterOrDigit(c)))
                return BadRequest("Invalid name");
            if (!model.Password.All(c => Char.IsLetterOrDigit(c)))
                return BadRequest("Invalid pass");


            // Create an HttpClient instance using the factory
            using (var client = _httpClientFactory.CreateClient())
            {
                // Set the base address of the API you want to call
                client.BaseAddress = new System.Uri("http://localhost:5001/");

                // Send a POST request to the login endpoint
                HttpResponseMessage response = await client.PostAsJsonAsync("api/Users/register", model);

                // Check if the response status code is 200 (OK)
                if (response.IsSuccessStatusCode)
                {
                    // You can deserialize the response content here if needed
                    var result = await response.Content.ReadFromJsonAsync<UserDTO>();
                    return Ok(result);
                }
                else
                {
                    return BadRequest("Register failed");
                }
            }
        }


        // DELETE : api/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                // Concaténez l'ID à l'URL de l'API
                string urlWithId = $"http://localhost:5001/api/Users/{id}";

                // Envoie la requête HTTP DELETE
                HttpResponseMessage response = await client.DeleteAsync(urlWithId);

                // Vérifie la réponse
                if (response.IsSuccessStatusCode)
                {
                    return Ok($"Success {id}");
                }
                else
                {
                    return BadRequest("Delete failed");

                }
            }
        }

    }
}
