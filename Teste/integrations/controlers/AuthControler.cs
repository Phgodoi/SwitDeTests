using Microsoft.AspNetCore.Mvc.Testing;
using minimal_api;
using minimal_api.API.Domain.DTOs;
using minimal_api.API.Domain.Enuns;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace TestsAPi.Integrations.Controllers
{
    public class AuthController : IClassFixture<WebApplicationFactory<Startup>>, IAsyncLifetime
    {
        private readonly WebApplicationFactory<Startup> _applicationFactory;
        private readonly ITestOutputHelper _testOutput;
        private readonly HttpClient _httpClient;

        public AuthController(WebApplicationFactory<Startup> applicationFactory, ITestOutputHelper testOutput)
        {
            _applicationFactory = applicationFactory;
            _testOutput = testOutput;
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

            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            var httpClientRequest = await _httpClient.PostAsync("api/Auth/login", content);

            var response = JsonConvert.DeserializeObject<AdministradorLogado>(httpClientRequest.Content.ReadAsStringAsync().GetAwaiter().GetResult());


            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.Equal(login.Email, response?.Email);
        }

        [Fact]
        public async Task LoginWhenAuthIsInvalid()
        {
            var login = new LoginDTO
            {
                Email = "Administrador@teste.com",
                Password = "1234"
            };
            var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");

            var httClientRequest = await _httpClient.PostAsync("api/Auth/login", content);

            var response = JsonConvert.DeserializeObject<AdministradorLogado>(httClientRequest.Content.ReadAsStringAsync().GetAwaiter().GetResult());

            Assert.Equal(HttpStatusCode.Unauthorized, httClientRequest.StatusCode);
            Assert.NotEqual(login.Email, response?.Email);

        }
        [Fact]
        public async Task RegisterNewUser()
        {
            var Auth = new LoginDTO
            {
                Email = "Administrador@teste.com",
                Password = "12345"
            };

            var contentAuth = new StringContent(JsonConvert.SerializeObject(Auth), Encoding.UTF8, "application/json");
            var httpClientRequestAuth = await _httpClient.PostAsync("api/Auth/login", contentAuth);           
            var responseAuth = JsonConvert.DeserializeObject<AdministradorLogado>(await httpClientRequestAuth.Content.ReadAsStringAsync());
           

            var registerNewUser = new AdministradorDTO
            {
                Email = "AdministradorTest2@teste.com",
                Password = "12345",
                Profile = Profiles.Adm
            };

            var content = new StringContent(JsonConvert.SerializeObject(registerNewUser), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseAuth?.Token);

            var httpClientRequest = await _httpClient.PostAsync("api/Administradores", content);
           
            var responseNewUser = JsonConvert.DeserializeObject<AdministradorDTO>(await httpClientRequest.Content.ReadAsStringAsync());
            Assert.NotNull(responseNewUser);
            Assert.Equal(registerNewUser.Email, responseNewUser?.Email);
        }

        public async Task InitializeAsync()
        {
            var Auth = new LoginDTO
            {
                Email = "Administrador@teste.com",
                Password = "12345"
            };

            var contentAuth = new StringContent(JsonConvert.SerializeObject(Auth), Encoding.UTF8, "application/json");
            var httpClientRequestAuth = await _httpClient.PostAsync("api/Auth/login", contentAuth);
            var responseAuth = JsonConvert.DeserializeObject<AdministradorLogado>(await httpClientRequestAuth.Content.ReadAsStringAsync());


            var registerNewUser = new AdministradorDTO
            {
                Email = "AdministradorTest2@teste.com",
                Password = "12345",
                Profile = Profiles.Adm
            };

            var content = new StringContent(JsonConvert.SerializeObject(registerNewUser), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseAuth?.Token);

            var httpClientRequest = await _httpClient.PostAsync("api/Administradores", content);

            var responseNewUser = JsonConvert.DeserializeObject<AdministradorDTO>(await httpClientRequest.Content.ReadAsStringAsync());
            Assert.NotNull(responseNewUser);
            Assert.Equal(registerNewUser.Email, responseNewUser?.Email);
        }

        public Task DisposeAsync()
        {
            throw new NotImplementedException();
        }
    }
}
