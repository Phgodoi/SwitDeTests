using AutoBogus;
using Microsoft.AspNetCore.Mvc.Testing;
using minimal_api;
using minimal_api.API.Domain.DTOs;
using minimal_api.API.Domain.Entities;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using TestsAPi.Integrations.Controllers;
using Xunit;
using Xunit.Abstractions;

namespace TestsAPi.integrations.controlers
{
    public class VeiculosTestsController : AuthTestsControler
    {
        protected Veiculo _newVeiculo = new();

        public VeiculosTestsController(WebApplicationFactory<Startup> applicationFactory, ITestOutputHelper testOutput)
            : base(applicationFactory, testOutput)
        {

        }

        [Fact]
        public async Task RegisteNewVeicle()
        {
            var registerNewVeiculo = AutoFaker();

            var content = new StringContent(JsonConvert.SerializeObject(registerNewVeiculo), Encoding.UTF8, "application/json");
            var httpClientRequest = await _httpClient.PostAsync("api/Veiculos/CriarVeiculo", content);

            _newVeiculo = JsonConvert.DeserializeObject<Veiculo>(await httpClientRequest.Content.ReadAsStringAsync()) ?? new();

            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

        [Fact]
        public async Task GetVeiculos()
        {
            var httpClientRequest = await _httpClient.GetAsync($"api/Veiculos/GetVeiculos", HttpCompletionOption.ResponseHeadersRead);
            var Veiculos = JsonConvert.DeserializeObject<IList<Veiculo>>(await httpClientRequest.Content.ReadAsStringAsync());

            Assert.Equal(HttpStatusCode.OK, httpClientRequest.StatusCode);
            Assert.NotNull(Veiculos);
        }
        public VeiculoDTO AutoFaker()
        {
            return new AutoFaker<VeiculoDTO>()
                        .RuleFor(r => r.Marca, fk => fk.Vehicle.Manufacturer())
                        .RuleFor(r => r.Nome, fk => fk.Vehicle.Model())
                        .RuleFor(r => r.Ano, fk => fk.Date.Past(50).Year).Generate();
        }
    }
}
