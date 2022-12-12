using MyFinance.Domain.Interfaces;
using MyFinance.Infra.Context;

namespace MyFinance.Infra.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyfinancesContext _context;
        public IUserRepository Users { get; }
        

        public UnitOfWork(MyfinancesContext context, IUserRepository Users)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.Users = Users ?? throw new ArgumentNullException(nameof(Users));
            
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
