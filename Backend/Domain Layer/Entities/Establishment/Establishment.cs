namespace WebApplication1.Domain_Layer.Entities
{
    public interface IEstablishment : IEstablishment_Sale, IEstablishment_Table, IEstablishment_Item, IEstablishment_Information
    {
        string GetName();
        void SetName(string name);
    }

    public partial class Establishment : EntityBase
    {
        public string Name { get; set; }
        public virtual EstablishmentInformation Information { get; set; } = new EstablishmentInformation();
        public virtual ICollection<Item> Items { get; set; } = new List<Item>();
        public virtual ICollection<Table> Tables { get; set; } = new List<Table>();
        public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();

        public Establishment()
        {

        }


        public Establishment(
            string name)
        {
            this.SetName(name);
        }

        public void SetName(string name)
        {
            this.EstablishmentNameMustBeValid(name);
            this.Name = name;
        }

        public string GetName()
        {
            return this.Name;
        }

        //Checkers and validators

        protected void EstablishmentNameMustBeValid(string name)
        {
            if (!this.IsNameValid(name))
            {
                throw new ArgumentException("Establishment name is not valid");
            }
        }
        public bool IsNameValid(string name)
        {
            if (name == "")
            {
                return false;
            }
            return true;
        }

    }
}
