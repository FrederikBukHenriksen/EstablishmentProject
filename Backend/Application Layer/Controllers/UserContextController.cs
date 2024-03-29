﻿using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using WebApplication1.Services;

namespace WebApplication1.ControllersBui
{
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/user-context/")]
    public class UserContextController : ControllerBase
    {
        [HttpGet("get-accessible-establishments")]
        public List<Guid> GetAccessibleEstablishments([FromServices] IUserContextService userContextService)
        {
            var establishments = userContextService.GetUser().UserRoles.Select(x => x.Establishment).ToList();
            List<Guid> list = establishments.Select(x => x.Id).ToList();
            return list;
        }
    }
}