using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;

namespace EstablishmentProject.test
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
            var insertedEstablishment = new Establishment
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Cafe 1",
            };
            _repository.Add(insertedEstablishment);

            // Assert
            var fetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(fetchedEstablishment);
        }

        [Fact]
        public void GetEntity_Succes()
        {
            // Arrange
            var insertedEstablishment = new Establishment
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "Cafe 2",
            };
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
            var insertedEstablishment = new Establishment
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000003"),
                Name = "Cafe 3",
            };
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();
            var databaseFetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            _repository.Remove(databaseFetchedEstablishment);

            // Assert
            var fectedEstablishmentAfterDelete = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.Null(fectedEstablishmentAfterDelete);
        }

        [Fact]
        public void UpdateEntityProperty_Succes()
        {
            // Arrange
            var insertedEstablishment = new Establishment
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000004"),
                Name = "Cafe 4",
            };
            dbContext.Set<Establishment>().Add(insertedEstablishment);
            dbContext.SaveChanges();
            var databaseFetchedEstablishment = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(databaseFetchedEstablishment);

            // Act
            databaseFetchedEstablishment.Name = "Cafe 4.1";
            _repository.Update(databaseFetchedEstablishment);

            // Assert
            var fectedEstablishmentAfterUpdate = dbContext.Set<Establishment>().Find(insertedEstablishment.Id);
            Assert.NotNull(fectedEstablishmentAfterUpdate);
            Assert.Equal(fectedEstablishmentAfterUpdate.Id, Guid.Parse("00000000-0000-0000-0000-000000000004"));
            Assert.Equal("Cafe 4.1", fectedEstablishmentAfterUpdate.Name);
        }

        [Fact]
        public void FindEntity_Success()
        {
            // Arrange
            var insertedEstablishment = new Establishment
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000005"),
                Name = "Cafe 5",
            };
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
            { new Establishment{
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000006"),
                    Name = "Common name"
                },
                new Establishment{
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000007"),
                    Name = "Common name"
                },
                new Establishment{
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000008"),
                    Name = "Different name"
                }
            };
            dbContext.Set<Establishment>().AddRange(insertedEstablishments);
            dbContext.SaveChanges();
            var databaseFectedEstablishments = dbContext.Set<Establishment>().ToList();
            Assert.True(databaseFectedEstablishments.Any(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000006")));
            Assert.True(databaseFectedEstablishments.Any(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000007")));


            // Act
            var repositoryFetchedEstablishments = _repository.FindAll(x => x.Name == "Common name");

            // Assert
            Assert.Equal(2, repositoryFetchedEstablishments.ToList().Count);
            Assert.True(databaseFectedEstablishments.Any(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000006")));
            Assert.True(databaseFectedEstablishments.Any(x => x.Id == Guid.Parse("00000000-0000-0000-0000-000000000007")));

        }


    }
}
