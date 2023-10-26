namespace WebApplication1.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        Establishment getItAll(Guid id);
    }
    public class EstablishmentRepository : GenericRepository<Establishment>, IEstablishmentRepository
    {

        public EstablishmentRepository(IDatabaseContext context) : base(context)
        {
        }

        Establishment IEstablishmentRepository.getItAll(Guid id)
        {
            context.haha();
            var res = context.Set<Establishment>().Include(x => x.Tables).Include(x => x.Location).Where(x => x.Id == id).FirstOrDefault();
            return res;
        }
    }
}