using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Repositories
{
    public interface IDatabaseContext
    {
        int SaveChanges();
        void Dispose();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
