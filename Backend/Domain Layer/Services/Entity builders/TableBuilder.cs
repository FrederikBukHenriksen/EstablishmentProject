using WebApplication1.Domain_Layer.Services.Entity_builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public interface ITableBuilder : IEntityBuilder<Table>
    {
        ITableBuilder WithName(string name);
    }

    public class TableBuilder : EntityBuilderBase<Table>, ITableBuilder
    {

        private string? builderName = null;

        public override void ReadPropertiesOfEntity(Table entity)
        {
            this.builderName = entity.Name;
        }

        public override void WritePropertiesOfEntity(Table Entity)
        {
            Entity.Name = (string)this.builderName;
        }

        public ITableBuilder WithName(string name)
        {
            this.builderName = name;
            return this;
        }
        public override bool Validation()
        {
            return true;
        }


    }
}

