using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Domain_Layer.Entities
{
    public abstract class EntityBase
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

    }


}
