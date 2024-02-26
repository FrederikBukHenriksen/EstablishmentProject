using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.CommandHandlerServices;
using WebApplication1.CommandHandlers;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;

namespace EstablishmentProject.test.Application_Test.HandlerService_Test
{
    public class HandlerVerifyItems_Test : IntegrationTest
    {
        private Establishment establishment1;
        private Establishment establishment2;
        private VerifyItemsCommandService validator;
        private IUnitOfWork unitOfWork;
        private IUserContextService userContextService;

        public HandlerVerifyItems_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            validator = scope.ServiceProvider.GetRequiredService<VerifyItemsCommandService>();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            userContextService = scope.ServiceProvider.GetRequiredService<IUserContextService>();
            CommonArrange();
        }

        private void CommonArrange()
        {
            var user = new User("Frederik@mail.com", "12345678");
            establishment1 = new Establishment("Test establishment1");
            establishment2 = new Establishment("Test establishment2");

            var estab1item = establishment1.CreateItem("Coffee", 20);
            establishment1.AddItem(estab1item);

            var estab2item = establishment2.CreateItem("Tea", 10);
            establishment2.AddItem(estab2item);

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
        public async void VerifyItems_WithValidItemsFromEstablishment_ShouldPassValidation()
        {
            // Arrange
            var command = new CommandTestObject
            {
                EstablishmentId = establishment1.Id,
                ItemsIds = establishment1.GetItems().Select(x => x.Id).ToList()
            };

            // Act
            validator.Verify(command);

            // Assert
            Assert.NotNull(true);
        }

        [Fact]
        public async void VerifyItems_WithInvalidItemsFromEstablishment_ShouldNotPassValidation()
        {
            // Arrange
            var command = new CommandTestObject
            {
                EstablishmentId = establishment1.Id,
                ItemsIds = establishment2.GetItems().Select(x => x.Id).ToList()
            };

            // Act
            Action act = () => validator.Verify(command);

            // Assert
            Assert.Throws<UnauthorizedAccessException>(act);

        }

        private class CommandTestObject : ICommand, ICmdField_ItemsIds
        {
            public Guid EstablishmentId { get; set; }
            public List<Guid> ItemsIds { get; set; }
        }

        private class TestHandler : IHandler<CommandTestObject, TestReturn>
        {
            public Task<TestReturn> Handle(CommandTestObject command)
            {
                return Task.FromResult(new TestReturn());
            }
        }

        private class TestReturn : IReturn
        {
        }
    }
}
