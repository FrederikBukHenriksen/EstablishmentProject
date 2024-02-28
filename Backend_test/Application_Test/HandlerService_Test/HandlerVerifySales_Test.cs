using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.CommandHandlerServices;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;
namespace EstablishmentProject.test.Application_Test.HandlerService_Test
{

    public class HandlerVerifySales_Test : IntegrationTest
    {
        private Establishment establishment1;
        private Establishment establishment2;
        private VerifySalesCommandService validator;
        private IUnitOfWork unitOfWork;
        private IUserContextService userContextService;

        public HandlerVerifySales_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            validator = scope.ServiceProvider.GetRequiredService<VerifySalesCommandService>();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            userContextService = scope.ServiceProvider.GetRequiredService<IUserContextService>();
            CommonArrange();
        }

        private void CommonArrange()
        {
            var user = new User("Frederik@mail.com", "12345678");
            establishment1 = new Establishment("Test establishment1");
            establishment2 = new Establishment("Test establishment2");

            var estab1sale = establishment1.CreateSale(DateTime.Now);
            establishment1.AddSale(estab1sale);

            var estab2sale = establishment2.CreateSale(DateTime.Now);
            establishment2.AddSale(estab2sale);

            var userRole1 = user.CreateUserRole(establishment1, user, Role.Admin);
            user.AddUserRole(userRole1);

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment1);
                uow.establishmentRepository.Add(establishment2);
                uow.userRepository.Add(user);
            }

            userContextService.SetUser(user);
        }

        [Fact]
        public void VerifySales_WithValidSalesFromEstablishment_ShouldPassValidation()
        {
            // Arrange
            var command = new CommandTestObject
            {
                EstablishmentId = establishment1.Id,
                SalesIds = establishment1.GetSales().Select(x => x.Id).ToList()
            };

            // Act
            validator.Verify(command);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void VerifySales_WithInvalidSalesFromEstablishment_ShouldNotPassValidation()
        {
            // Arrange
            var command = new CommandTestObject
            {
                EstablishmentId = establishment1.Id,
                SalesIds = establishment2.GetSales().Select(x => x.Id).ToList()
            };

            // Act
            Action act = () => validator.Verify(command);

            // Assert
            Assert.Throws<UnauthorizedAccessException>(act);
        }

        private class CommandTestObject : ICommand, ICmdField_SalesIds
        {
            public Guid EstablishmentId { get; set; }
            public List<Guid> SalesIds { get; set; }
        }
    }
}
