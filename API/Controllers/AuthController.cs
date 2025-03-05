using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using minimal_api.API.Domain.DTOs;
using minimal_api.API.Domain.Interfaces;

namespace minimal_api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdministradorService _administradorService;

        public AuthController(IAuthService authService, IAdministradorService administradorService)
        {
            _authService = authService;
            _administradorService = administradorService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [Tags("Auth")]
        public IActionResult Login([FromBody] LoginDTO loginDTO)
        {
            var adm = _administradorService.Login(loginDTO);

            if (adm != null)
            {
                string token = _authService.GerarToken(adm);
                return Ok(new AdministradorLogado
                {
                    Email = adm.Email,
                    Profile = adm.Profile,
                    Token = token
                });
            }
            else
                return Unauthorized();
        }

    }
}
