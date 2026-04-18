using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void ApplyGlobalQueryFilters(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Diploma>().HasQueryFilter(d => !d.IsDeleted);
            modelBuilder.Entity<Quiz>().HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<Question>().HasQueryFilter(q => !q.IsDeleted);
            modelBuilder.Entity<AnswerOption>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<Enrollment>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<QuizAttempt>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<AttemptAnswer>().HasQueryFilter(a => !a.IsDeleted);
            modelBuilder.Entity<OtpCode>().HasQueryFilter(o => !o.IsDeleted);
        }
    }
}
