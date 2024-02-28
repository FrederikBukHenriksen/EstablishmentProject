using WebApplication1.Domain_Layer.Entities;

namespace WebApplication1.Infrastructure_Layer.DataTransferObjects
{
    public class TableDTO
    {
        public Guid Id;
        public string Name { get; set; }

        public TableDTO(Table table)
        {
            this.Id = table.Id;
            this.Name = table.Name;
        }
    }
}
