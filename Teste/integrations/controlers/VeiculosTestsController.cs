using Microsoft.AspNetCore.Mvc.Testing;
using minimal_api;
using minimal_api.API.Domain.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using AutoBogus;
using minimal_api.API.Domain.Entities;
using TestsAPi.Integrations.Controllers;

namespace TestsAPi.integrations.controlers
{
    public class VeiculosTestsController  : AuthTestsControler
    {      
        protected Veiculo _newVeiculo = new();

        public VeiculosTestsController(WebApplicationFactory<Startup> applicationFactory, ITestOutputHelper testOutput)
            : base(applicationFactory, testOutput)
        {
          
        }

        [Fact]
        public async Task RegisteNewVeicle()
        {
            var registerNewVeiculo = new AutoFaker<VeiculoDTO>()
                        .RuleFor(r => r.Marca, fk => fk.Vehicle.Manufacturer())
                        .RuleFor(r => r.Nome, fk => fk.Vehicle.Model())
                        .RuleFor(r => r.Ano, fk => fk.Date.Past(50).Year).Generate();

            var content = new StringContent(JsonConvert.SerializeObject(registerNewVeiculo), Encoding.UTF8, "application/json");
            var httpClientRequest = await _httpClient.PostAsync("api/Veiculos", content);
            Assert.Equal(HttpStatusCode.Created, httpClientRequest.StatusCode);
        }

    }
}
