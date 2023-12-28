using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Application_Layer.Services
{
    public interface IUnitOfWork
    {
        public void SaveChanges();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext dbContext;

        public UnitOfWork([FromServices] ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void SaveChanges()
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
                    throw; //rethrow the error after catching it.
                }
            }
        }

    }
}
