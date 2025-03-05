using minimal_api.API.Domain.Entities;

namespace minimal_api.API.Domain.Interfaces
{
    public interface IAuthService
    {
        string GerarToken(Administrador adm);
    }
}
