using DMIOpenData;
using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.CommandsQueriesHandlersReturns.Weather;
using WebApplication1.CommandHandlers;
using WebApplication1.Utils;

namespace EstablishmentProject.test.Application_Test.Handlers_Test.Weather_Test
{
    public class Weather_test : IntegrationTest
    {
        private IHandler<GetTemperatureCommand, GetWeatherReturn> handler;

        public Weather_test() : base()
        {
            handler = scope.ServiceProvider.GetRequiredService<IHandler<GetTemperatureCommand, GetWeatherReturn>>();

        }


        [Fact]
        public async Task GetTemperature_WithHourResolution_ShouldReturnTemperature()
        {
            //Arrange
            GetTemperatureCommand command = new GetTemperatureCommand
            {
                coordinates = new Coordinates(55.6761, 12.5683),
                start = new DateTime(2021, 1, 1, 0, 0, 0),
                end = new DateTime(2021, 1, 7, 0, 0, 0),
                TimeResolution = TimeResolution.Hour
            };

            //Act
            var result = await handler.Handle(command);

            //Assert
            Assert.Equal(24 * 6, result.data.Count);
            Assert.All(result.data, x => Assert.True(x.Item2 > -40 && x.Item2 < 40));
        }

        [Fact]
        public async Task GetTemperature_WithDailyResolution_ShouldReturnTemperature()
        {
            //Arrange
            GetTemperatureCommand command = new GetTemperatureCommand
            {
                coordinates = new Coordinates(55.6761, 12.5683),
                start = new DateTime(2021, 1, 1, 0, 0, 0),
                end = new DateTime(2021, 1, 7, 0, 0, 0),
                TimeResolution = TimeResolution.Date
            };

            //Act
            var result = await handler.Handle(command);

            //Assert
            Assert.Equal(6, result.data.Count);
            Assert.All(result.data, x => Assert.True(x.Item2 > -40 && x.Item2 < 40));
        }
    }
}