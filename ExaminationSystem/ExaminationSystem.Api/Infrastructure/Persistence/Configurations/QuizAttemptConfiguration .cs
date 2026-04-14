using ExaminationSystem.Api.Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.Property(a => a.Score).HasPrecision(5, 2);
            builder.HasIndex(a => new { a.StudentId, a.QuizId })
                   .HasFilter("[Status] = 0")
                   .IsUnique();

            builder.HasIndex(a => new { a.StudentId, a.StartedAt }); 
        }

    }
}
