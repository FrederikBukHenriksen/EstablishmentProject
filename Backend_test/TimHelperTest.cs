using EstablishmentProject.test.TestingCode;
using WebApplication1.Utils;

namespace EstablishmentProject.test
{
    public class TimHelperTest : BaseTest
    {
        public TimHelperTest() : base(new List<ITestService>())
        {
        }

        //Timeline

        [Fact]
        public void CreateTimelineAsList_ShouldThrowArgumentExceptionWhenEndIsEarlierThanStart()
        {
            // Arrange
            DateTime start = new DateTime(2022, 1, 5, 0, 0, 0);
            DateTime end = new DateTime(2022, 1, 1, 0, 0, 0);
            TimeResolution resolution = TimeResolution.Hour;

            // Act
            ArgumentException exception = Assert.Throws<ArgumentException>(() => TimeHelper.CreateTimelineAsList(start, end, resolution));

            //Assert
            Assert.Equal("End must be equal or later than start", exception.Message);
        }

        [Fact]
        public void CreateTimeLineAsList_WithHour()
        {
            // Arrange
            DateTime start = new DateTime(2022, 1, 1, 0, 0, 0);
            DateTime end = new DateTime(2022, 1, 5, 0, 0, 0);
            TimeResolution resolution = TimeResolution.Hour;

            // Act
            List<DateTime> result = TimeHelper.CreateTimelineAsList(start, end, resolution);

            // Assert
            Assert.Equal(5 * 24, result.Count);
        }

        public void CreateTimeLineAsList_WithDate()
        {
            // Arrange
            DateTime start = new DateTime(2022, 1, 1, 0, 0, 0);
            DateTime end = new DateTime(2022, 1, 5, 0, 0, 0);
            TimeResolution resolution = TimeResolution.Date;

            // Act
            List<DateTime> result = TimeHelper.CreateTimelineAsList(start, end, resolution);

            // Assert
            Assert.Equal(5, result.Count);
        }

        public void CreateTimeLineAsList_WithMonth()
        {
            // Arrange
            DateTime start = new DateTime(2022, 1, 1, 0, 0, 0);
            DateTime end = new DateTime(2022, 5, 1, 0, 0, 0);
            TimeResolution resolution = TimeResolution.Month;

            // Act
            List<DateTime> result = TimeHelper.CreateTimelineAsList(start, end, resolution);

            // Assert
            Assert.Equal(5, result.Count);
        }

        public void CreateTimeLineAsList_WithYear()
        {
            // Arrange
            DateTime start = new DateTime(2022, 1, 1, 0, 0, 0);
            DateTime end = new DateTime(2025, 1, 1, 0, 0, 0);
            TimeResolution resolution = TimeResolution.Year;

            // Act
            List<DateTime> result = TimeHelper.CreateTimelineAsList(start, end, resolution);

            // Assert
            Assert.Equal(4, result.Count);
        }

        //Add to date with timeresolution

        [Fact]
        public void AddToDateTime_ShouldReturnDateTimeWithAddedAmount()
        {
            // Arrange
            DateTime datetime = new DateTime(2022, 1, 1, 12, 30, 45);
            int amount = 3;
            TimeResolution timeResolution = TimeResolution.Month;

            // Act
            DateTime result = datetime.AddToDateTime(amount, timeResolution);

            // Assert
            switch (timeResolution)
            {
                case TimeResolution.Hour:
                    Assert.Equal(datetime.AddHours(amount), result);
                    break;
                case TimeResolution.Date:
                    Assert.Equal(datetime.AddDays(amount), result);
                    break;
                case TimeResolution.Month:
                    Assert.Equal(datetime.AddMonths(amount), result);
                    break;
                case TimeResolution.Year:
                    Assert.Equal(datetime.AddYears(amount), result);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Fact]
        public void AddToDateTime_ShouldThrowArgumentOutOfRangeExceptionForInvalidTimeResolution()
        {
            // Arrange
            DateTime datetime = new DateTime(2022, 1, 1, 12, 30, 45);
            int amount = 3;
            TimeResolution timeResolution = (TimeResolution)99; // Invalid value

            // Act & Assert
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(() => datetime.AddToDateTime(amount, timeResolution));

            // Additional assertions on the exception if needed...
            Assert.Equal("Specified argument was out of the range of valid values.", exception.Message);
        }
    }
}
