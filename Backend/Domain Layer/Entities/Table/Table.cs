namespace WebApplication1.Domain_Layer.Entities
{

    public interface ITable
    {
        string GetName();
        string SetName(string name);
    }
    public class Table : EntityBase, ITable
    {
        public Guid EstablishmentId { get; set; }
        public string Name { get; set; }

        public Table()
        {

        }

        public Table(Establishment establishment, string name)
        {
            this.EstablishmentId = establishment.Id;
            this.Name = name;
        }

        public string SetName(string name)
        {
            this.TableNameMustBeValid(name);
            this.Name = name;
            return this.GetName();
        }
        public string GetName()
        {
            return this.Name;
        }

        //Checkers and validators

        public void TableNameMustBeValid(string name)
        {
            if (!this.IsTableNameValid(name))
            {
                throw new ArgumentException("Table name is not valid");
            }
        }
        public bool IsTableNameValid(string name)
        {
            if (name == "")
            {
                return false;
            }
            return true;
        }

    }
}
