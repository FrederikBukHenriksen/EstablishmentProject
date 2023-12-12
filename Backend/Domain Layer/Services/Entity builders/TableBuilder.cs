using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain.Entities
{
    public interface ITableBuilder : IEntityBuilder<Table>
    {
        ITableBuilder WithName(string name);
    }

    public class TableBuilder : EntityBuilderBase<Table>, ITableBuilder
    {
        public ITableBuilder WithName(string name)
        {
            Entity.Name = name;
            return this;
        }
        public override bool EntityValidation()
        {
            return true;
        }
    }
}

