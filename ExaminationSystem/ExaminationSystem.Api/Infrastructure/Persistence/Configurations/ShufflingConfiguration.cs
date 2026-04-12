using ExaminationSystem.Api.Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Configurations
{
    public class ShufflingConfiguration : IEntityTypeConfiguration<AttemptQuestionOrder>, IEntityTypeConfiguration<AttemptOptionOrder>
    {
        public void Configure(EntityTypeBuilder<AttemptQuestionOrder> builder)
        {
            builder.HasKey(aq => new { aq.AttemptId, aq.QuestionId });

            builder.HasOne(aq => aq.Attempt)
                   .WithMany()
                   .HasForeignKey(aq => aq.AttemptId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(aq => aq.Question)
                   .WithMany()
                   .HasForeignKey(aq => aq.QuestionId)
                   .OnDelete(DeleteBehavior.NoAction); 
        }

        public void Configure(EntityTypeBuilder<AttemptOptionOrder> builder)
        {
            builder.HasKey(ao => new { ao.AttemptId, ao.OptionId });

            builder.HasOne(ao => ao.Attempt)
                   .WithMany()
                   .HasForeignKey(ao => ao.AttemptId)
                   .OnDelete(DeleteBehavior.NoAction); 

            builder.HasOne(ao => ao.Option)
                   .WithMany()
                   .HasForeignKey(ao => ao.OptionId)
                   .OnDelete(DeleteBehavior.NoAction); 
        }
    }
}


