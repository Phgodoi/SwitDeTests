using AutoBogus;
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

             _registerNewUser = new AutoFaker<AdministradorDTO>()
                    .RuleFor(r => r.Email, fk => fk.Person.Email);
      

            var content = new StringContent(JsonConvert.SerializeObject(_registerNewUser), Encoding.UTF8, "application/json");
            var httpClientRequest = await _httpClient.PostAsync("api/Administradores", content);

            var responseNewUser = JsonConvert.DeserializeObject<AdministradorDTO>(await httpClientRequest.Content.ReadAsStringAsync());
            Assert.NotNull(responseNewUser?.Email);
            Assert.Equal(_registerNewUser.Email, responseNewUser?.Email);
        }


    }
}
