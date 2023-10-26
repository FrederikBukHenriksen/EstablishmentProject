namespace WebApplication1.Repositories
{
    public interface IDatabaseContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

    }
}
