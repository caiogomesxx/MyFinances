using MyFinance.Domain.Entities;
using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using MyFinance.Infra.Utils;
using Microsoft.AspNet.Identity;

namespace MyFinance.Infra.Repositories
{
    public class UserRepository : GenericRepository<TbUsuario>, IUserRepository
    {
        public UserRepository(MyfinancesContext context) : base(context)
        {
        }
        public void CadastrarUser(TbUsuario user) 
        {
            try 
            {
                var TbUsuario = _context.TbUsuarios.Where(x => x.DsEmail== user.DsEmail).FirstOrDefault();
                if (TbUsuario != null)
                    throw Exception("Esse email já é cadastrado!");

                user.DsSenha = CriptographPassword.HashValue(user.DsSenha);

                _context.TbUsuarios.Add(user);
                _context.SaveChanges(); 
            }
            catch (Exception ex) 
            {
                throw ex;
            }

        }
        public async Task<TbUsuario> GetUserByEmail(string email, string senha) 
        {
            try 
            {
                senha = CriptographPassword.HashValue(senha);
                return await _context.TbUsuarios.Where( x=> x.DsEmail == email && x.DsSenha == senha).FirstOrDefaultAsync();
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
