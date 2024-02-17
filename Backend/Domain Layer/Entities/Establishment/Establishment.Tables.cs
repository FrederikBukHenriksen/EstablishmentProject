namespace WebApplication1.Domain_Layer.Entities
{

    public interface IEstablishment_Table
    {
        Table CreateTable(string name);
        void RemoveTable(Table table);
        List<Table> GetTables();
        void AddTable(Table table);
        void SetTableName(Table table, string name);
    }

    public partial class Establishment : IEstablishment_Table
    {
        public Table CreateTable(string name)
        {
            Table table = new Table(name);
            return table;
        }

        public void AddTable(Table table)
        {
            this.TableMustBeCreatedForEstablishment(table);
            this.TableMustNotAlreadyExist(table);
            this.TableNameMustBeUnique(table.Name);
            this.Tables.Add(table);
        }

        public void RemoveTable(Table table)
        {
            this.TableMustExist(table);
            this.TableMustNotBeUsedInSales(table);
            this.Tables.Remove(table);
        }

        public List<Table> GetTables()
        {
            return this.Tables.ToList();
        }

        public void SetTableName(Table table, string name)
        {
            this.TableNameMustBeUnique(name);
            table.SetName(name);
        }


        //Checkers and validators

        protected void TableMustExist(Table table)
        {
            if (!this.doesTableExist(table))
            {
                throw new InvalidOperationException("Table does not exist within the establishment");
            }
        }
        protected void TableMustNotAlreadyExist(Table table)
        {
            if (this.doesTableExist(table))
            {
                throw new InvalidOperationException("Table already exists within the establishment");
            }
        }

        private bool IsTableCreatedForEstablishment(Table table)
        {
            return table.EstablishmentId == this.Id;
        }

        private void TableMustBeCreatedForEstablishment(Table table)
        {
            if (!this.IsTableCreatedForEstablishment(table))
            {
                throw new InvalidOperationException("Table is not created for establishment");
            }
        }

        private bool IsTableUsedInSales(Table table)
        {
            return this.GetSales().Any(x => x.GetSalesTables().Any(x => x.Table == table));
        }

        private void TableMustNotBeUsedInSales(Table table)
        {
            if (this.IsTableUsedInSales(table))
            {
                throw new InvalidOperationException("Table is used in sales, and therefore cannot be deleted");
            }
        }

        private void TableNameMustBeUnique(string name)
        {
            if (this.IsNameAlreadyInUse(name))
            {
                throw new ArgumentException("Name is already in use");
            }
        }

        private bool IsNameAlreadyInUse(string name)
        {
            return this.GetTables().Any(x => x.Name == name);
        }
        public bool doesTableExist(Table table)
        {
            return this.GetTables().Any(x => x == table);
        }
    }
}
