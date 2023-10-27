namespace WebApplication1.Repositories
{

    public interface IEstablishmentRepository : IRepository<Establishment>
    {
        Establishment getItAll(Guid id);
    }
    public class EstablishmentRepository : Repository<Establishment>, IEstablishmentRepository
    {

        public EstablishmentRepository(IDatabaseContext context) : base(context)
        {
        }

        Establishment IEstablishmentRepository.getItAll(Guid id)
        {
            var res = context.Set<Establishment>().Include(x => x.Tables).Include(x => x.Location).Where(x => x.Id == id).FirstOrDefault();
            var table = new Table
            {
                Name = "Table HAHAHAH",
                Establishment = res,
            };

            var nyEstab = new Establishment() { Name = "Ny establishment" };

            Add(nyEstab);

            //var testtable =              new
            //    {
            //        Name = "Table 2",
            //        EstablishmentId = new Guid("00000000-0000-0000-0000-000000000001"),
            //    },

            var lul2 = context.Set<Table>().Add(table);
            var lul3 = context.Set<Table>().Add(table);
            context.SaveChanges();
            return res;
        }
    }
}