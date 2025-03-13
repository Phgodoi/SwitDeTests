using AutoBogus;
using Microsoft.AspNetCore.Mvc.Testing;
using minimal_api;
using minimal_api.API.Domain.DTOs;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace TestsAPi.Integrations.Controllers
{
    public class AuthTestsControler : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Startup> _applicationFactory;
        protected readonly ITestOutputHelper _testOutput;
        protected readonly HttpClient _httpClient;
        protected AdministradorLogado _administradorLogado = new();


        public AuthTestsControler(WebApplicationFactory<Startup> applicationFactory, ITestOutputHelper testOutput)
        {
            _applicationFactory = applicationFactory;
            _testOutput = testOutput;
            _httpClient = _applicationFactory.CreateClient();
        }
        public async Task InitializeAsync()
        {
            await LoginWhenAuthIsValid();
        }

        [Fact]
        public async Task LoginWhenAuthIsValid()
        {
            var login = new LoginDTO
            {
                Email = "Administrador@teste.com",
                Password = "12345"
            };

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            var httpClientRequest = await _httpClient.PostAsync("api/Auth/login", content);

            var response = JsonConvert.DeserializeObject<AdministradorLogado>( await httpClientRequest.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.Equal(login.Email, response?.Email);

            _administradorLogado = response ?? new();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _administradorLogado?.Token);


        }

        [Fact]
        public async Task LoginWhenAuthIsInvalid()
        {
            var login = new AutoFaker<LoginDTO>()
                    .RuleFor(r => r.Email, fk => fk.Person.Email).Generate();

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            var httClientRequest = await _httpClient.PostAsync("api/Auth/login", content);

            var response = JsonConvert.DeserializeObject<AdministradorLogado>( await httClientRequest.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.Unauthorized, httClientRequest.StatusCode);

        }       
        public Task DisposeAsync()
        {
            _httpClient.Dispose();
            return Task.CompletedTask;
        }
    }
}
