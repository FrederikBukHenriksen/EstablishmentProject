using NodaTime;
using WebApplication1.Domain_Layer.Entities;

namespace EstablishmentProject.test.Domain.Entities_Test.Establishment_Test
{
    public class EstablishmentTest
    {
        [Fact]
        public void SetOpeningHours_ShouldSetOpeningHours()
        {
            // Arrange
            var establishment = new Establishment();
            var dayOfWeek = DayOfWeek.Monday;
            var openTime = new LocalTime(9, 0);
            var closeTime = new LocalTime(17, 0);

            // Act
            var result = establishment.SetOpeningHours(dayOfWeek, openTime, closeTime);

            // Assert
            Assert.NotNull(result);

            var openingHoursMonday = result.Find(x => x.dayOfWeek == dayOfWeek);
            Assert.Equal(dayOfWeek, openingHoursMonday.dayOfWeek);
            Assert.Equal(openTime, openingHoursMonday.open);
            Assert.Equal(closeTime, openingHoursMonday.close);
        }
        [Fact]
        public void GetAllOpeningHours_ShouldReturnOpeningHours()
        {
            // Arrange
            var establishment = new Establishment();
            var dayOfWeek = DayOfWeek.Monday;
            var openTime = new LocalTime(9, 0);
            var closeTime = new LocalTime(17, 0);
            establishment.SetOpeningHours(dayOfWeek, openTime, closeTime);

            // Act
            var result = establishment.GetAllOpeningHours();

            // Assert
            Assert.NotNull(result);
            var openingHoursMonday = result.Find(x => x.dayOfWeek == dayOfWeek);
            Assert.Equal(dayOfWeek, openingHoursMonday.dayOfWeek);
            Assert.Equal(openTime, openingHoursMonday.open);
            Assert.Equal(closeTime, openingHoursMonday.close);
        }

        [Fact]
        public void GetOpeningHours()
        {
            // Arrange
            var establishment = new Establishment();
            var dayOfWeek = DayOfWeek.Monday;
            var openTime = new LocalTime(9, 0);
            var closeTime = new LocalTime(17, 0);
            establishment.SetOpeningHours(dayOfWeek, openTime, closeTime);

            // Act
            var result = establishment.GetOpeningHours(dayOfWeek);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dayOfWeek, result.dayOfWeek);
            Assert.Equal(openTime, result.open);
            Assert.Equal(closeTime, result.close);
        }
    }


}
