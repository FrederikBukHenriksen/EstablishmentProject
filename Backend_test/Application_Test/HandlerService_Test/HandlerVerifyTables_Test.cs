﻿using EstablishmentProject.test.TestingCode;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Application_Layer.Services;
using WebApplication1.Application_Layer.Services.CommandHandlerServices;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain_Layer.Entities;
using WebApplication1.Services;

namespace EstablishmentProject.test.Application_Test.HandlerService_Test
{
    public class HandlerVerifyTables_Test : IntegrationTest
    {
        private Establishment establishment1;
        private Establishment establishment2;
        private VerifyTablesCommandService validator;
        private IUnitOfWork unitOfWork;
        private IUserContextService userContextService;

        public HandlerVerifyTables_Test() : base(new List<ITestService> { DatabaseTestContainer.CreateAsync().Result })
        {
            validator = (VerifyTablesCommandService)scope.ServiceProvider.GetRequiredService<IVerifyTablesCommandService>();
            unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            userContextService = scope.ServiceProvider.GetRequiredService<IUserContextService>();
            CommonArrange();
        }

        private void CommonArrange()
        {
            var user = new User("Frederik@mail.com", "12345678");
            establishment1 = new Establishment("Test establishment1");
            establishment2 = new Establishment("Test establishment2");

            var estab1table = establishment1.CreateTable("Table1");
            establishment1.AddTable(estab1table);
            var estab2table = establishment2.CreateTable("Table2");
            establishment2.AddTable(estab2table);

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
        public void VerifyItems_WithValidItemsFromEstablishment_ShouldPassValidation()
        {
            // Arrange
            var command = new CommandTestObject
            {
                EstablishmentId = establishment1.Id,
                TablesIds = establishment1.GetTables().Select(x => x.Id).ToList()
            };

            // Act
            validator.VerifyTables(command);

            // Assert
            Assert.True(true);
        }

        [Fact]
        public void VerifyItems_WithInvalidItemsFromEstablishment_ShouldNotPassValidation()
        {
            // Arrange
            var command = new CommandTestObject
            {
                EstablishmentId = establishment1.Id,
                TablesIds = establishment2.GetTables().Select(x => x.Id).ToList()
            };

            // Act
            Action act = () => validator.VerifyTables(command);

            // Assert
            Assert.Throws<UnauthorizedAccessException>(act);
        }

        private class CommandTestObject : ICommand, ICmdField_TablesIds
        {
            public Guid EstablishmentId { get; set; }
            public List<Guid> TablesIds { get; set; }
        }
    }
}