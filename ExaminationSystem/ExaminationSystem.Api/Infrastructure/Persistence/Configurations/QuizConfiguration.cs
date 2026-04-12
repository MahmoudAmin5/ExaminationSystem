using ExaminationSystem.Api.Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasIndex(q => new { q.DiplomaId, q.Status }); 
            builder.Property(q => q.PassScore).HasDefaultValue(60.00m);
            builder.Property(q => q.PassScore).HasPrecision(5, 2);

        }
   
    }
}
