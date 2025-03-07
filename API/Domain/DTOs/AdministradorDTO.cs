using minimal_api.API.Domain.Enuns;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace minimal_api.API.Domain.DTOs
{
    public class AdministradorDTO
    {
        [EmailAddress]
        public string Email { get; set; } = default!;        
        public string Password { get; set; } = default!;
        public Profiles? Profile { get; set; } = default!;

    }
}
