using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Namotion.Reflection;
using System.Net;
using System.Reflection.Emit;

namespace WebApplication1.Models
{
    public class EntityBase
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }

}
