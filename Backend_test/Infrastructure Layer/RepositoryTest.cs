using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Domain_Layer.Services.Repositories;

namespace EstablishmentProject.test.Repositories
{
    public class RepositoryTest : BaseIntegrationTest
    {
        private readonly IEstablishmentRepository _repository;

        public RepositoryTest(IntegrationTestWebAppFactory factory) : base(factory)
        {
            _repository = scope.ServiceProvider.GetRequiredService<IEstablishmentRepository>();
        }

        [Fact]
        public void AddEntity_Succes()
        {
            // Arrange
            var fetchToVerifyDatabase = dbContext.Set<Establishment>().Find(Guid.Parse("00000000-0000-0000-0000-000000000001"));
            Assert.Null(fetchToVerifyDatabase);

            // Act
            var insertedEstablishment = new Establishment("Cafe 1");
            insertedEstablishment.Id = Guid.Parse("00000000-0000-0000-0000-000000000001");
            _repository.Add(insertedEstablishment);

            // Assert
            var fetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(fetchedEstablishment);
        }

        [Fact]
        public void GetEntity_Succes()
        {
            // Arrange
            var insertedEstablishment = new Establishment("Cafe 2");
            insertedEstablishment.Id = Guid.Parse("00000000-0000-0000-0000-000000000002");
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();
            var databaseFetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            var fetchedEstablishment = _repository.GetById(insertedEstablishment.Id);

            // Assert
            Assert.NotNull(fetchedEstablishment);
            Assert.Equal(fetchedEstablishment.Id, Guid.Parse("00000000-0000-0000-0000-000000000002"));
            Assert.Equal(fetchedEstablishment.Name, "Cafe 2");
        }

        [Fact]
        public void RemoveEntity_Succes()
        {
            // Arrange
            var insertedEstablishment = new Establishment("Cafe 3");
            insertedEstablishment.Id = Guid.Parse("00000000-0000-0000-0000-000000000003");
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();
            var databaseFetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            _repository.Remove(databaseFetchedEstablishment);

            // Assert
            var fetchedEstablishmentAfterDelete = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.Null(fetchedEstablishmentAfterDelete);
        }

        [Fact]
        public void UpdateEntityProperty_Succes()
        {
            // Arrange
            var insertedEstablishment = new Establishment("Cafe 4");
            insertedEstablishment.Id = Guid.Parse("00000000-0000-0000-0000-000000000004");
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();
            var databaseFetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            databaseFetchedEstablishment.Name = "Cafe 4.1";
            _repository.Update(databaseFetchedEstablishment);

            // Assert
            var fetchedEstablishmentAfterUpdate = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(fetchedEstablishmentAfterUpdate);
            Assert.Equal(fetchedEstablishmentAfterUpdate.Id, Guid.Parse("00000000-0000-0000-0000-000000000004"));
            Assert.Equal("Cafe 4.1", fetchedEstablishmentAfterUpdate.Name);
        }

        [Fact]
        public void FindEntity_Success()
        {
            // Arrange
            var insertedEstablishment = new Establishment("Cafe 5");
            insertedEstablishment.Id = Guid.Parse("00000000-0000-0000-0000-000000000005");
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();
            var databaseFetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            var repositoryFetchedEstablishment = _repository.Find(x => x.Id == insertedEstablishment.Id);

            // Assert
            Assert.NotNull(repositoryFetchedEstablishment);
            Assert.Equal(repositoryFetchedEstablishment.Id, Guid.Parse("00000000-0000-0000-0000-000000000005"));
        }

        [Fact]
        public void FindAllEntities_Success()
        {
            // Arrange
            var insertedEstablishments = new Establishment[]
            {
                new Establishment("Common name"),
                new Establishment("Common name"),
                new Establishment("Different name")
            };
            insertedEstablishments[0].Id = Guid.Parse("00000000-0000-0000-0000-000000000006");
            insertedEstablishments[1].Id = Guid.Parse("00000000-0000-0000-0000-000000000007");
            insertedEstablishments[2].Id = Guid.Parse("00000000-0000-0000-0000-000000000008");
            dbContext.Set<Establishment>().AddRange(insertedEstablishments);
            dbContext.SaveChanges();
            var databaseFetchedEstablishments = dbContext.Set<Establishment>().ToList();
            Assert.True(databaseFetchedEstablishments.Any(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000006")));
            Assert.True(databaseFetchedEstablishments.Any(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000007")));

            // Act
            var repositoryFetchedEstablishments = _repository.FindAll(x => x.Name == "Common name");

            // Assert
            Assert.Equal(2, repositoryFetchedEstablishments.ToList().Count);
            Assert.True(databaseFetchedEstablishments.Any(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000006")));
            Assert.True(databaseFetchedEstablishments.Any(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000007")));
        }

        // Ensure you have the necessary dependencies and imports for Xunit and your testing infrastructure
    }
}
