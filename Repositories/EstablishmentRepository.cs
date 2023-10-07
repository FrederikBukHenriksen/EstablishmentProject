namespace WebApplication1.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
    }
    public class EstablishmentRepository : Repository<Establishment>, IEstablishmentRepository
    {

        public EstablishmentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}