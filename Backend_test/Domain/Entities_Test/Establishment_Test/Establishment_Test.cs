using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test
{
    public class Establishment_Test
    {
        [Fact]
        public void Constructor_ShouldCreateEstablishment()
        {
            // Arrange
            string name = "Test establishment";

            // Act

            Establishment establishment = new Establishment(name);

            // Assert
            Assert.NotNull(establishment);
            Assert.Equal(name, establishment.GetName());
        }

        [Fact]
        public void SetName_ShouldSetName()
        {
            // Arrange
            Establishment establishment = new Establishment();
            string name = "New name";

            // Act
            establishment.SetName(name);

            // Assert
            Assert.Equal(name, establishment.GetName());
        }

        public void SetName_WithEmptyName_ShouldNotSetName()
        {
            // Arrange
            Establishment establishment = new Establishment();
            string name = "";

            // Act
            Action act = () => establishment.SetName(name);

            // Assert
            Assert.Throws<ArgumentException>(act);
        }


        [Fact]
        public void GetName_ShouldReturnName()
        {
            // Arrange
            string name = "Test establishment";
            Establishment establishment = new Establishment(name);

            // Act
            var result = establishment.GetName();

            // Assert
            Assert.Equal(name, result);
        }
    }
}
