using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using minimal_api.API.Domain.DTOs;
using minimal_api.API.Domain.Entities;
using minimal_api.API.Domain.Enuns;
using minimal_api.API.Domain.Interfaces;
using minimal_api.API.Domain.ModelViews;

namespace minimal_api.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdministradoresController : ControllerBase
    {
        private readonly IAdministradorService _administradorService;
        private readonly IAuthService _authService;
        public AdministradoresController(IAdministradorService administradorService, IAuthService authService)
        {
            _administradorService = administradorService;
            _authService = authService;
        }

        [HttpPost]
        [Authorize(Roles = "Adm")]
        [Tags("Administradores")]
        public IActionResult CriarAdministrador([FromBody] AdministradorDTO administradorDTO)
        {
            var validacao = new ErrosDeValidacao { Menssagens = new List<string>() };

            if (string.IsNullOrEmpty(administradorDTO.Email)) validacao.Menssagens.Add("Email não pode ser vazio!");
            if (string.IsNullOrEmpty(administradorDTO.Password)) validacao.Menssagens.Add("Senha não pode ser vazia!");
            if (administradorDTO.Profile == null) validacao.Menssagens.Add("Perfil não pode ser vazio!");

            var adm = new Administrador
            {
                Email = administradorDTO.Email,
                Password = administradorDTO.Password,
                Profile = administradorDTO.Profile.ToString() ?? Profiles.Editor.ToString(),
            };

            _administradorService.Update(adm);

            var admView = new
            {
                Id = adm.Id,
                Email = adm.Email,
                Profile = adm.Profile,
            };

            return Created($"/administradores/{admView.Id}", admView);
        }

        [HttpGet]
        [Authorize(Roles = "Adm")]
        [Tags("Administradores")]
        public IActionResult GetAdministradores([FromQuery] int? pagina)
        {
            var adms = _administradorService.Get(pagina).Select(x => new
            {
                Id = x.Id,
                Email = x.Email,
                Profile = x.Profile,
            });

            return Ok(adms);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Adm")]
        [Tags("Administradores")]
        public IActionResult GetAdministradorPorId([FromQuery] int? pagina, int id)
        {
            var admById = _administradorService.Get(pagina).Where(x => x.Id == id).Select(x => new
            {
                Id = x.Id,
                Email = x.Email,
                Profile = x.Profile
            }).FirstOrDefault();

            if (admById != null)
                return Ok(admById);
            else
                return NotFound();
        }
    }
}
