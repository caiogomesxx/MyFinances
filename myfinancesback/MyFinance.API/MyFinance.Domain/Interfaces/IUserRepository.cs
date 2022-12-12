using MyFinance.Domain.Entities;
using MyFinance.Infra.Context;

namespace MyFinance.Domain.Interfaces
{
    public interface IUserRepository :IGenericRepository<TbUsuario>
    {
       void CadastrarUser(TbUsuario user);
       Task<TbUsuario> GetUserByEmail(string email, string senha);
    }
}


