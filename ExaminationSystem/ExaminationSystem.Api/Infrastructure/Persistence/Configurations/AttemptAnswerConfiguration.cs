using ExaminationSystem.Api.Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Configurations
{
    public class AttemptAnswerConfiguration : IEntityTypeConfiguration<AttemptAnswer>
    {
        public void Configure(EntityTypeBuilder<AttemptAnswer> builder)
        {
       
            builder.HasIndex(a => new { a.AttemptId, a.QuestionId }).IsUnique();

            builder.HasOne(a => a.Attempt)
              .WithMany(at => at.Answers)
              .HasForeignKey(a => a.AttemptId)
              .OnDelete(DeleteBehavior.NoAction); 

            
            builder.HasOne(a => a.Question)
                   .WithMany()
                   .HasForeignKey(a => a.QuestionId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    
    }
}
