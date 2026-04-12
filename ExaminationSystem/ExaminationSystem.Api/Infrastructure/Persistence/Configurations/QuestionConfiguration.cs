using ExaminationSystem.Api.Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Configurations
{
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasIndex(q => q.QuizId).HasFilter("[IsDeleted] = 0");
            builder.Property(q => q.OrderIndex).HasDefaultValue(1);
        }
    
    }
}
