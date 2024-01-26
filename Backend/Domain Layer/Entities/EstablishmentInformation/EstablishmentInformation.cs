using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebApplication1.Domain_Layer.Entities
{
    public class EstablishmentInformation : EntityBase
    {
        public EstablishmentInformation()
        {

        }
        public Location Location { get; set; } = new Location();
        public virtual ICollection<OpeningHours> OpeningHours { get; set; } = new List<OpeningHours>();


        internal void setOpeningHour(OpeningHours openingHours)
        {
            if (this.getOpeningHours().Any(x => x.dayOfWeek == openingHours.dayOfWeek))
            {
                OpeningHours openingHour = this.getOpeningHours().Find(x => x.dayOfWeek == openingHours.dayOfWeek)!;
                this.removeOpeningHour(openingHour);
            }
        }

        internal void removeOpeningHour(OpeningHours openingHour)
        {
            this.getOpeningHours().Remove(openingHour);
        }

        internal List<OpeningHours> getOpeningHours()
        {
            return this.OpeningHours.ToList();
        }



    }

    public class EstablishmentInformationConfiguration : IEntityTypeConfiguration<EstablishmentInformation>
    {
        public void Configure(EntityTypeBuilder<EstablishmentInformation> builder)
        {
            builder.OwnsOne(e => e.Location);





        }
    }
}
