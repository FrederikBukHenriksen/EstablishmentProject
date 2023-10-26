using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Repositories
{
    public interface IDatabaseContext
    {
        void haha();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
