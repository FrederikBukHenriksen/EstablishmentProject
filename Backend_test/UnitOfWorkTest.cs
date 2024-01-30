using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace EstablishmentProject.test
{
    public class UnitOfWorkTest : BaseIntegrationTest
    {
        private IUnitOfWork uow;

        public UnitOfWorkTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        }

        [Fact]
        public void RepositoryProperties_ReturnRepositories()
        {
            // Act
            var establishmentRepository = uow.establishmentRepository;
            var userRepository = uow.userRepository;
            var salesRepository = uow.salesRepository;
            var itemRepository = uow.itemRepository;

            // Assert
            Assert.IsType<EstablishmentRepository>(establishmentRepository);
            Assert.IsType<UserRepository>(userRepository);
            Assert.IsType<SalesRepository>(salesRepository);
            Assert.IsType<ItemRepository>(itemRepository);
        }

        [Fact]
        public void SuccesfullyUseUOW()
        {
            // Arrange
            Establishment establishment = new Establishment("Cafe 1");

            // Act
            using (var unitOfWork = uow)
            {
                uow.establishmentRepository.Add(establishment);
            }

            // Assert
            Assert.Equal(establishment, uow.establishmentRepository.GetById(establishment.Id));
        }

        [Fact]
        public void UnsuccesfullyUseUOW()
        {
            // Arrange
            Establishment establishment = new Establishment("Cafe 1");

            uow.establishmentRepository.Add(establishment);

            // Act
            using (var unitOfWork = uow)
            {
            }

            uow.establishmentRepository.Add(establishment);


            // Assert
            Assert.Empty(uow.establishmentRepository.GetAll());
        }


    }
}
