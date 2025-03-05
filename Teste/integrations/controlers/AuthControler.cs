using Microsoft.AspNetCore.Mvc.Testing;
using minimal_api;
using minimal_api.API.Domain.DTOs;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace TestsAPi.Integrations.Controllers
{
    public class AuthController : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _applicationFactory;
        private readonly HttpClient _httpClient;

        public AuthController(WebApplicationFactory<Startup> applicationFactory)
        {
            _applicationFactory = applicationFactory;
            _httpClient = _applicationFactory.CreateClient();
        }

        [Fact]
        public async Task LoginWhenAuthIsValid()
        {
            var login = new LoginDTO
            {
                Email = "Administrador@teste.com",
                Password = "12345"
            };

            var content = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");

            var httpClientRequest = await _httpClient.PostAsync("api/Auth/login", content);

            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
        }
    }
}
