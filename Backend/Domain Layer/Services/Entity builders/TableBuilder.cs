//using WebApplication1.Domain_Layer.Services.Entity_builders;

//namespace WebApplication1.Domain_Layer.Entities
//{
//    public interface ITableBuilder : IEntityBuilder<Table>
//    {
//        ITableBuilder WithName(string name);
//    }

//    public class TableBuilder : EntityBuilderBase<Table>, ITableBuilder
//    {

//        private string? builderName = null;

//        public override Table Build()
//        {
//            return new Table((string)this.builderName);

//        }

//        public ITableBuilder WithName(string name)
//        {
//            this.builderName = name;
//            return this;
//        }


//    }
//}

