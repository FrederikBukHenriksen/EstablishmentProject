using Microsoft.AspNetCore.Mvc;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace WebApplication1.Application_Layer.Services
{
    public interface IUnitOfWork : IDisposable
    {
        IEstablishmentRepository establishmentRepository { get; }
        IUserRepository userRepository { get; }
        ISalesRepository salesRepository { get; }
        IItemRepository itemRepository { get; }
    }

    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext dbContext;

        public IEstablishmentRepository establishmentRepository
        {
            get
            {
                return new EstablishmentRepository(this.dbContext);
            }
        }

        public IUserRepository userRepository
        {
            get
            {
                return new UserRepository(this.dbContext);
            }
        }

        public ISalesRepository salesRepository
        {
            get
            {
                return new SalesRepository(this.dbContext);
            }
        }

        public IItemRepository itemRepository
        {
            get
            {
                return new ItemRepository(this.dbContext);
            }
        }

        public UnitOfWork([FromServices] ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            if (this.validation(this.dbContext)) this.SaveChanges();
        }

        private void SaveChanges()
        {
            using (var transaction = this.dbContext.Database.BeginTransaction())
            {
                try
                {
                    this.dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw; //rethrow exceptions thrown in the process of saving
                }
            }
        }

        private bool validation(DbContext dbContext)
        {
            return true;
        }

    }
}
