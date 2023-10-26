namespace WebApplication1.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        Establishment getItAll(Guid id);
    }
    public class EstablishmentRepository : GenericRepository<Establishment>, IEstablishmentRepository
    {

        public EstablishmentRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Establishment getItAll(Guid id)
        {
            var res = context.Set<Establishment>().Where(x => x.Id == id).FirstOrDefault();
            return res;
        }
    }
}