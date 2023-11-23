using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.CommandHandlers;
using WebApplication1.Data;
using WebApplication1.Data.DataModels;
using WebApplication1.Models;
using WebApplication1.Repositories;
using WebApplication1.Services;
using Xunit.Abstractions;

namespace WebApplication1.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/correlation/")]
    public class CorrelationController : ControllerBase
    {
        private IUserRepository _userRepository;
        private ISalesRepository _salesRepository;
        private IUserRolesRepository _userRolesRepository;
        private IEstablishmentRepository _establishmentRepository;

        public CorrelationController(IEstablishmentRepository establishmentRepository, IUserRepository userRepository, IUserRolesRepository userRolesRepository, ISalesRepository salesRepository)
        {
            _salesRepository = salesRepository;
            _userRolesRepository = userRolesRepository;
            _userRepository = userRepository;
            _establishmentRepository = establishmentRepository;
        }

        [HttpPost("CorrelationCoefficientAndLag")]
        public void CorrelationCoefficientAndLag([FromServices] ICommandHandler<CorrelationBetweenSalesAndWeatherCommand, (TimeSpan, double)> handler)
        {
            var command = new CorrelationBetweenSalesAndWeatherCommand { StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now};

            var res = handler.Execute(command);
        }

        [HttpPost("CorrelationGraph")]
        public void CorrelationGraph([FromServices] ICommandHandler<CorrelationBetweenSalesAndWeatherCommand, (TimeSpan, double)> handler)
        {
            var command = new CorrelationBetweenSalesAndWeatherCommand { StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now };

            var res = handler.Execute(command);
        }
    }
}