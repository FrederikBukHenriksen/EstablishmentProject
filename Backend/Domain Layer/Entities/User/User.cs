﻿using Microsoft.EntityFrameworkCore;
using Namotion.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Domain.Entities
{
    public class User : EntityBase
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
