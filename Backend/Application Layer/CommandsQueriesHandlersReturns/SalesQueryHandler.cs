﻿using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using WebApplication1.Application_Layer.Objects;
using WebApplication1.CommandsHandlersReturns;
using WebApplication1.Domain.Entities;
using WebApplication1.Domain.Services.Repositories;
using WebApplication1.Services;
using WebApplication1.Utils;

namespace WebApplication1.CommandHandlers
{
    public class SalesQuery : CommandBase
    {
        public SalesSortingParameters? salesSortingParameters { get; set; }
        //public List<TimePeriod> TimePeriods { get; set; }
    }

    public class SalesQueryReturn : ReturnBase
    {
        public List<Sale> Sales { get; set; }
    }

    public class SalesQueryHandler : HandlerBase<SalesQuery, SalesQueryReturn>
    {
        private IEstablishmentRepository establishmentRepository;
        private ISalesRepository salesRepository;
        private IUserContextService userContextService;

        public SalesQueryHandler(IEstablishmentRepository establishmentRepository, IUserContextService userContextService, ISalesRepository salesRepository)
        {
            this.establishmentRepository = establishmentRepository;
            this.salesRepository = salesRepository;
            this.userContextService = userContextService;
        }   

        public override SalesQueryReturn Handle(SalesQuery command) 
        {
            Establishment activeEstablishment = userContextService.GetActiveEstablishment();

            List<Sale> sales = establishmentRepository.GetEstablishmentSales(userContextService.GetActiveEstablishment().Id).ToList();
            sales = salesRepository.IncludeSalesItems(sales);
            //Sort by time period
            //sales.SortSalesByTimePeriods(command.TimePeriods);

            //Sort by items
            if (command.salesSortingParameters != null)
            {
                sales = SalesSortingParametersExecute.SortSales(sales, command.salesSortingParameters);
            }


            //List<(DateTime, IEnumerable<Sale?>)> timelineWithSales = TimeHelper.mapToATimeline(sales, x => x.GetTimeOfSale(), command.TimePeriods, command.TimeResolution).ToList();
            //List<TimeAndValue<int>> res = timelineWithSales.Select(x => new TimeAndValue<int> { dateTime = x.Item1, value = x.Item2.Count() }).ToList();
            return new SalesQueryReturn { Sales = sales.Select(x => new Sale()).ToList() };
        }
    }
}