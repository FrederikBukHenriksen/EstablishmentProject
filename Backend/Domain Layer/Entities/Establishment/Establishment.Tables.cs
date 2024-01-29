namespace WebApplication1.Domain_Layer.Entities
{

    public interface IEstablishment_Table
    {
        Table CreateTable(string name);
        void RemoveTable(Table table);
        List<Table> GetTables();
    }

    public partial class Establishment : EntityBase, IEstablishment_Table
    {
        public Table CreateTable(string name)
        {
            Table table = new Table(name);
            if (this.IsNameEmpty(name))
            {
                throw new ArgumentException("Name cannot be empty");
            }
            this.AddTable(table);
            return table;
        }

        private void AddTable(Table table)
        {
            this.Tables.Add(table);
        }

        public void RemoveTable(Table table)
        {
            if (this.IsTableUsedInSales(table))
            {
                throw new Exception("Table is used in sales, and therefore cannot be deleted");
            }
            this.Tables.Remove(table);
        }

        public List<Table> GetTables()
        {
            return this.Tables.ToList();
        }


        //Checkers and validators
        private bool IsTableUsedInSales(Table table)
        {
            return this.GetSales().Any(x => x.Table == table);
        }

        private bool IsNameEmpty(string name)
        {
            return string.IsNullOrEmpty(name);
        }
    }
}
