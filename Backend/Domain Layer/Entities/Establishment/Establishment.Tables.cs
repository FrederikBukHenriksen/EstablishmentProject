namespace WebApplication1.Domain_Layer.Entities
{
    public partial class Establishment
    {
        public void AddTable(Table table)
        {
            this.Tables.Add(table);
        }

        public void RemoveTable(Table table)
        {
            this.Tables.Remove(table);
        }

        public ICollection<Table> GetTables()
        {
            return this.Tables;
        }
    }
}
