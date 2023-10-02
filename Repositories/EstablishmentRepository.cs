namespace WebApplication1.Repositories
{
    public class EstablishmentRepository : Repository<Establishment>, IEstablishmentRepository
    {
        public EstablishmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
