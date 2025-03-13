using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using minimal_api.API.Domain.DTOs;
using minimal_api.API.Domain.Entities;
using minimal_api.API.Domain.Interfaces;
using minimal_api.API.Domain.ModelViews;

namespace minimal_api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VeiculosController : ControllerBase
    {
        private readonly IVeiculoService _veiculoService;

        public VeiculosController(IVeiculoService veiculoService)
        {
            _veiculoService = veiculoService;
        }

        private ErrosDeValidacao Validar(VeiculoDTO veiculoDTO)
        {
            var validacao = new ErrosDeValidacao { Menssagens = new List<string>() };

            if (string.IsNullOrEmpty(veiculoDTO.Nome)) validacao.Menssagens.Add("O Nome não pode ser vázio!");
            if (string.IsNullOrEmpty(veiculoDTO.Marca)) validacao.Menssagens.Add("A Marca não pode ser vázia!");
            if (veiculoDTO.Ano < 1950 || veiculoDTO.Ano > DateTime.Now.Year) validacao.Menssagens.Add($"Ano inválido, o interválo aceito é de 1950 até {DateTime.Now.Year}");

            return validacao;
        }

        [HttpPost]
        [Authorize(Roles = "Adm, Editor")]
        [Tags("Veiculos")]
        [Route("CriarVeiculo")]
        public IActionResult CriarVeiculo([FromBody] VeiculoDTO veiculoDTO)
        {
            var validacao = Validar(veiculoDTO);
            if (validacao.Menssagens.Count() > 0) return BadRequest(validacao);

            var veiculo = new Veiculo
            {
                Nome = veiculoDTO.Nome,
                Marca = veiculoDTO.Marca,
                Ano = veiculoDTO.Ano,
            };

            _veiculoService.Save(veiculo);

            return Created($"/veiculos/{veiculo.Id}", veiculo);
        }

        [HttpGet]
        [Authorize(Roles = "Adm, Editor")]
        [Tags("Veiculos")]
        [Route("GetVeiculos")]
        public IActionResult GetVeiculos([FromQuery] int? pagina)
        {
            var veiculos = _veiculoService.GetBy(pagina);
            return Ok(veiculos);
        }

        [HttpGet]
        [Authorize(Roles = "Adm, Editor")]
        [Tags("Veiculos")]
        [Route("GetVeiculoPorId{id}")]
        public IActionResult GetVeiculoPorId(int id)
        {
            var veiculo = _veiculoService.GetById(id);
            if (veiculo == null) return NotFound();

            return Ok(veiculo);
        }

        [HttpPut]
        [Authorize(Roles = "Adm")]
        [Tags("Veiculos")]
        [Route("AtualizarVeiculo{id}")]
        public IActionResult AtualizarVeiculo(int id, [FromBody] VeiculoDTO veiculoDTO)
        {
            var veiculo = _veiculoService.GetById(id);
            if (veiculo == null) return NotFound();

            var validacao = Validar(veiculoDTO);
            if (validacao.Menssagens.Count() > 0) return BadRequest(validacao);

            veiculo.Nome = veiculoDTO.Nome;
            veiculo.Marca = veiculoDTO.Marca;
            veiculo.Ano = veiculoDTO.Ano;

            _veiculoService.Update(veiculo);

            return Ok(veiculo);
        }

        [HttpDelete]
        [Authorize(Roles = "Adm")]
        [Tags("Veiculos")]
        [Route("DeletarVeiculo{id}")]
        public IActionResult DeletarVeiculo(int id)
        {
            var veiculo = _veiculoService.GetById(id);
            if (veiculo == null) return NotFound();

            _veiculoService.Delete(veiculo);
            return NoContent();
        }
    }
}
