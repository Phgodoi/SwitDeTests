using AutoBogus;
using Microsoft.AspNetCore.Mvc.Testing;
using minimal_api;
using minimal_api.API.Domain.DTOs;
using minimal_api.API.Domain.Entities;
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
    public class AdministradoresController : AuthTestsControler
    {
        protected AdministradorDTO _registerNewUser = new();
        public AdministradoresController(WebApplicationFactory<Startup> applicationFactory, ITestOutputHelper testOutput)
            : base(applicationFactory, testOutput)
        {

        }
        [Fact]
        public async Task RegisterNewUser()
        {
            _registerNewUser = AutoFaker();

            var content = new StringContent(JsonConvert.SerializeObject(_registerNewUser), Encoding.UTF8, "application/json");
            var httpClientRequest = await _httpClient.PostAsync("api/Administradores/CriarAdministrador", content);

            var responseNewUser = JsonConvert.DeserializeObject<AdministradorDTO>(await httpClientRequest.Content.ReadAsStringAsync());
            Assert.NotNull(responseNewUser?.Email);
            Assert.Equal(_registerNewUser.Email, responseNewUser?.Email);
        }
        [Fact]
        public async Task RegisterNewUserAndGetById()
        {

            _registerNewUser = AutoFaker();

            var content = new StringContent(JsonConvert.SerializeObject(_registerNewUser), Encoding.UTF8, "application/json");
            var httpClientRequest = await _httpClient.PostAsync("api/Administradores/CriarAdministrador", content);

            var responseNewUser = JsonConvert.DeserializeObject<Administrador>(await httpClientRequest.Content.ReadAsStringAsync());

            var getById = await _httpClient.GetAsync($"api/Administradores/GetAdministradores{responseNewUser?.Id}", HttpCompletionOption.ResponseHeadersRead);
            var getByIdResponse = JsonConvert.DeserializeObject<Administrador>(await getById.Content.ReadAsStringAsync());

            Assert.NotNull(responseNewUser?.Email);
            Assert.Equal(_registerNewUser.Email, responseNewUser?.Email);

            Assert.Equal(HttpStatusCode.OK, getById.StatusCode);
            Assert.Equal(responseNewUser?.Id, getByIdResponse?.Id);
        }

        [Fact]
        public async Task GetAll()
        {
            var httpClientRequest = await _httpClient.GetAsync($"api/Administradores/GetAdministradores", HttpCompletionOption.ResponseHeadersRead);
            var response = JsonConvert.DeserializeObject<IList<Administrador>>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
        }
        public AdministradorDTO AutoFaker()
        {
            return new AutoFaker<AdministradorDTO>()
                    .RuleFor(r => r.Email, fk => fk.Person.Email).Generate();
        }

    }
}
