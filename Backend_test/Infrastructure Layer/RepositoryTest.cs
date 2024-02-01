using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace EstablishmentProject.test.Repositories
{
    public class RepositoryTest : BaseIntegrationTest
    {
        private IEstablishmentRepository repository;
        private IUnitOfWork unitOfWork;

        public RepositoryTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            clearDatabase();
            //repository = scope.ServiceProvider.GetRequiredService<IEstablishmentRepository>();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        }

        [Fact]
        public void AddEntity_Succes()
        {
            // Arrange
            List<Establishment> databaseFetchedEstablishment = dbContext.Set<Establishment>().ToList();
            Assert.False(databaseFetchedEstablishment.Any(x => x.Name == "Cafe 1"));

            // Act
            var insertedEstablishment = new Establishment("Cafe 1");
            using (unitOfWork)
            {
                unitOfWork.establishmentRepository.Add(insertedEstablishment);
            }

            // Assert
            var fetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(fetchedEstablishment);
        }


        [Fact]
        public void AddRange()
        {
            // Arrange
            List<Establishment> databaseFetchedEstablishments = dbContext.Set<Establishment>().ToList();
            Assert.False(databaseFetchedEstablishments.Any(x => x.Name == "Cafe 1.1"));
            Assert.False(databaseFetchedEstablishments.Any(x => x.Name == "Cafe 1.2"));

            // Act
            var insertedEstablishments = new Establishment[]
            {
                new Establishment("Cafe 1.1"),
                new Establishment("Cafe 1.2")
            };
            using (unitOfWork)
            {
                unitOfWork.establishmentRepository.AddRange(insertedEstablishments);
            }

            // Assert
            var fetchedEstablishment1 = dbContext.Set<Establishment>().Find(insertedEstablishments[0].Id);
            var fetchedEstablishment2 = dbContext.Set<Establishment>().Find(insertedEstablishments[1].Id);
            Assert.NotNull(fetchedEstablishment1);
            Assert.NotNull(fetchedEstablishment2);
        }

        [Fact]
        public void GetById_Succes()
        {
            // Arrange
            var insertedEstablishment = new Establishment("Cafe 2");
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();


            var databaseFetchedEstablishments = dbContext.Set<Establishment>().ToList();
            var databaseFetchedEstablishment = databaseFetchedEstablishments.Find(x => x.Name == "Cafe 2");
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            var fetchedEstablishment = unitOfWork.establishmentRepository.GetById(insertedEstablishment.Id);

            // Assert
            Assert.NotNull(fetchedEstablishment);
            Assert.Equal(insertedEstablishment.Id, fetchedEstablishment.Id);
            Assert.Equal(insertedEstablishment.Name, fetchedEstablishment.Name);
        }

        [Fact]
        public void RemoveEntity_Succes()
        {
            // Arrange
            var insertedEstablishment = new Establishment("Cafe 3");

            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();

            var databaseFetchedEstablishments = dbContext.Set<Establishment>().ToList();
            var databaseFetchedEstablishment = databaseFetchedEstablishments.Find(x => x.Name == "Cafe 3");
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            using (unitOfWork)
            {
                unitOfWork.establishmentRepository.Remove(databaseFetchedEstablishment);
            }

            // Assert
            Assert.False(dbContext.Set<Establishment>().ToList().Any(x => x.Name == insertedEstablishment.Name));
        }

        [Fact]
        public void UpdateEntityProperty_Succes()
        {
            // Arrange
            var insertedEstablishment = new Establishment("Cafe 4");
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();

            var databaseFetchedEstablishmentLists = dbContext.Set<Establishment>().ToList();
            var databaseFetchedEstablishment = databaseFetchedEstablishmentLists.Find(x => x.Name == "Cafe 4");
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            var newName = "Cafe 4.1";
            databaseFetchedEstablishment.Name = newName;
            using (unitOfWork)
            {
                unitOfWork.establishmentRepository.Update(databaseFetchedEstablishment);
            }

            // Assert
            var fetchedEstablishmentAfterUpdate = dbContext.Set<Establishment>().ToList().Find(x => x.Name == newName);
            Assert.NotNull(fetchedEstablishmentAfterUpdate);
            Assert.Equal(insertedEstablishment.Id, fetchedEstablishmentAfterUpdate.Id);
            Assert.Equal(newName, fetchedEstablishmentAfterUpdate.Name);
        }

        [Fact]
        public void FindEntity_Success()
        {
            // Arrange
            var insertedEstablishment = new Establishment("Cafe 5");
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();

            var databaseFetchedEstablishmentLists = dbContext.Set<Establishment>().ToList();
            var databaseFetchedEstablishment = databaseFetchedEstablishmentLists.Find(x => x.Name == "Cafe 5");
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            var repositoryFetchedEstablishment = unitOfWork.establishmentRepository.Find(x => x.Id == insertedEstablishment.Id);

            // Assert
            Assert.NotNull(repositoryFetchedEstablishment);
            Assert.Equal(repositoryFetchedEstablishment.Id, repositoryFetchedEstablishment.Id);
        }

        [Fact]
        public void FindAllEntities_Success()
        {
            // Arrange
            List<Establishment> databaseFetchedEstablishment = dbContext.Set<Establishment>().ToList();
            Assert.False(databaseFetchedEstablishment.Any(x => x.Name == "Common name"));
            Assert.False(databaseFetchedEstablishment.Any(x => x.Name == "Different name"));


            var insertedEstablishments = new Establishment[]
            {
                new Establishment("Common name"),
                new Establishment("Common name"),
                new Establishment("Different name")
            };

            using (unitOfWork)
            {
                unitOfWork.establishmentRepository.AddRange(insertedEstablishments);
            }

            // Act
            var repositoryFetchedEstablishments = unitOfWork.establishmentRepository.FindAll(x => x.Name == "Common name");

            // Assert
            Assert.Equal(2, repositoryFetchedEstablishments.ToList().Count);
            Assert.True(repositoryFetchedEstablishments.Any(x => x.Id == insertedEstablishments[0].Id));
            Assert.True(repositoryFetchedEstablishments.Any(x => x.Id == insertedEstablishments[1].Id));

        }

        [Fact]
        public void GetAll()
        {
            // Arrange
            List<Establishment> databaseFetchedEstablishment = dbContext.Set<Establishment>().ToList();
            Assert.False(databaseFetchedEstablishment.Any(x => x.Name == "Cafe 6"));
            Assert.False(databaseFetchedEstablishment.Any(x => x.Name == "Cafe 7"));

            //Act
            var insertedEstablishments = new Establishment[]
            {
                new Establishment("Cafe 6"),
                new Establishment("Cafe 7")
            };
            dbContext.Set<Establishment>().AddRange(insertedEstablishments);
            dbContext.SaveChanges();

            // Assert
            var fetchedEstablishments = unitOfWork.establishmentRepository.GetAll();
            Assert.Equal(2, fetchedEstablishments.ToList().Count);
            Assert.True(fetchedEstablishments.Any(x => x.Id == insertedEstablishments[0].Id));
            Assert.True(fetchedEstablishments.Any(x => x.Id == insertedEstablishments[1].Id));
        }
    }
}
