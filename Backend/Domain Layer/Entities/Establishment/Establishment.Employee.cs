namespace WebApplication1.Domain_Layer.Entities
{
    public partial class Establishment
    {
        public void AddEmployee(Employee employee)
        {
            this.Employees.Add(employee);
        }

        public void RemoveEmployee(Employee employee)
        {
            this.Employees.Remove(employee);
        }
    }
}
