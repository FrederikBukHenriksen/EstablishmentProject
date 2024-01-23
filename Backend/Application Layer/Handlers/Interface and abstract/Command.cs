using WebApplication1.Utils;

namespace WebApplication1.CommandsHandlersReturns
{
    public interface ICommand
    {
    }
    public abstract class CommandBase : ICommand
    {
    }

    public interface ICmdField_EstablishmentId
    {
        public Guid EstablishmentId { get; set; }
    }

    public interface ICmdField_EstablishmentIds
    {
        public List<Guid> EstablishmentIds { get; set; }
    }

    public interface ICmdField_SalesIds : ICmdField_EstablishmentId
    {
        public List<Guid> SalesIds { get; set; }
    }


    public interface ICmdField_ItemsId : ICmdField_EstablishmentId
    {
        public List<Guid> ItemsIds { get; set; }

    }
    public interface ICmdField_SalesSorting
    {
        public SalesSorting Sorting { get; set; }

    }


}
