using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace EstablishmentProject.test
{
    public class UnitOfWorkTest : IntegrationTest
    {
        private IUnitOfWork uow;

        public UnitOfWorkTest() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        }

        [Fact]
        public void AccessRepository_ReturnRepositories()
        {
            // Act
            var establishmentRepository = uow.establishmentRepository;
            var userRepository = uow.userRepository;

            // Assert
            Assert.IsType<EstablishmentRepository>(establishmentRepository);
            Assert.IsType<UserRepository>(userRepository);

        }

        [Fact]
        public void UsingUnitOfWorkToSave_SuccesfullyUseUOW()
        {
            // Arrange
            Establishment establishment = new Establishment("Cafe 1");

            // Act
            using (var unitOfWork = uow)
            {
                uow.establishmentRepository.Add(establishment);
            }

            // Assert
            Assert.Equal(establishment.Id, uow.establishmentRepository.GetById(establishment.Id).Id);
        }

        [Fact]
        public void UsingUnitOfWorkToSave_UnsuccesfullyUseUOW()
        {
            // Arrange
            Establishment establishment = new Establishment("Cafe 1");

            // Act
            uow.establishmentRepository.Add(establishment);


            // Assert
            Assert.Empty(uow.establishmentRepository.GetAll().ToList());
        }


    }
}
