using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;

namespace EstablishmentProject.test.Application_Test.HandlerService_Test
{
    public class HandlerVerifyEstablishment_Test : IntegrationTest
    {
        private Establishment establishment1;
        private Establishment establishment2;
        private Establishment establishment3;
        private VerifyEstablishmentCommandService validator;
        private IUnitOfWork unitOfWork;
        private IUserContextService userContextService;

        public HandlerVerifyEstablishment_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            validator = (VerifyEstablishmentCommandService)scope.ServiceProvider.GetRequiredService<IVerifyEstablishmentCommandService>();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            userContextService = scope.ServiceProvider.GetRequiredService<IUserContextService>();
            CommonArrange();
        }

        private void CommonArrange()
        {
            var user = new User("Frederik@mail.com", "12345678");
            establishment1 = new Establishment("Test establishment1");
            establishment2 = new Establishment("Test establishment2");
            establishment3 = new Establishment("Test establishment3");

            var userRole1 = user.CreateUserRole(establishment1, user, Role.Admin);
            user.AddUserRole(userRole1);

            var userRole2 = user.CreateUserRole(establishment2, user, Role.Admin);
            user.AddUserRole(userRole2);

            using (var uow = unitOfWork)
            {
                uow.establishmentRepository.Add(establishment1);
                uow.establishmentRepository.Add(establishment2);
                uow.establishmentRepository.Add(establishment3);
                uow.userRepository.Add(user);
            }

            userContextService.SetUser(user);
        }

        [Fact]
        public void VerifyEstablishment_WithValidEstablishment_ShouldPassValidation()
        {
            //Arrange
            var command = new CommandTestObject { EstablishmentId = establishment1.Id };

            //Act
            validator.VerifyEstablishment(command);
        }

        [Fact]
        public void VerifyEstablishment_WithInvalidEstablishment_ShouldNotPassValidation()
        {
            //Arrange
            var command = new CommandTestObject { EstablishmentId = establishment3.Id };

            //Act
            Action act = () => validator.VerifyEstablishment(command);

            //Assert
            Assert.Throws<UnauthorizedAccessException>(act);
        }
        [Fact]
        public void VerifyEstablishment_WithValidEstablishments_ShouldPassValidation()
        {
            //Arrange
            var command = new CommandTestObjectMultiple { EstablishmentIds = new List<Guid> { establishment1.Id, establishment2.Id } };

            //Act
            validator.VerifyEstablishment(command);
        }

        [Fact]
        public void VerifyEstablishment_WithInvalidEstablishments_ShouldNotPassValidation()
        {
            //Arrange
            var command = new CommandTestObjectMultiple { EstablishmentIds = new List<Guid> { establishment1.Id, establishment3.Id } };

            //Act
            Action act = () => validator.VerifyEstablishment(command);

            //Assert
            Assert.Throws<UnauthorizedAccessException>(act);
        }

        private class CommandTestObject : ICommand, ICmdField_EstablishmentId
        {
            public Guid EstablishmentId { get; set; }
        }

        private class CommandTestObjectMultiple : ICommand, ICmdField_EstablishmentIds
        {
            public List<Guid> EstablishmentIds { get; set; }
        }


    }

}