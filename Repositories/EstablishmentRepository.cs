using System.Linq.Expressions;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class EstablishmentRepository : Repository<Establishment>, IEstablishmentRepository
    {
        public EstablishmentRepository(ApplicationDbContext context) : base(context) 
        {
        }
    }
}
