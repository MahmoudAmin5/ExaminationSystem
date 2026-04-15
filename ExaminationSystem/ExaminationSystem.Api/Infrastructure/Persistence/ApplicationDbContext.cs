using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Domain.Entities;
using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Infrastructure.Persistence.Seeding;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExaminationSystem.Api.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }


       
        public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
        public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
        public DbSet<UserActivityLog> UserActivityLogs => Set<UserActivityLog>();
        public DbSet<Diploma> Diplomas => Set<Diploma>();
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<AnswerOption> AnswerOptions => Set<AnswerOption>();
        public DbSet<Enrollment> Enrollments => Set<Enrollment>();
        public DbSet<QuizAttempt> QuizAttempts => Set<QuizAttempt>();
        public DbSet<AttemptAnswer> AttemptAnswers => Set<AttemptAnswer>();
        public DbSet<AttemptQuestionOrder> AttemptQuestionOrders => Set<AttemptQuestionOrder>();
        public DbSet<AttemptOptionOrder> AttemptOptionOrders => Set<AttemptOptionOrder>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            base.OnModelCreating(modelBuilder);

          
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            DbSeeder.SeedData(modelBuilder);

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
       
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
            {
                switch (entry.State)
                {
                   

                    case EntityState.Added:
                      
                        if (entry.Entity is BaseEntity<Guid> or BaseEntity<int> || entry.Entity is User)
                        {
                            entry.Entity.GetType().GetProperty("CreatedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                        }
                        break;

                    case EntityState.Modified:

                        var createdAtProperty = entry.Entity.GetType().GetProperty("CreatedAt");
                        if (createdAtProperty != null)
                        {
                            var createdAt = (DateTime)createdAtProperty.GetValue(entry.Entity)!;
                            if (DateTime.UtcNow.Subtract(createdAt).TotalSeconds > 2)
                            {
                                entry.Entity.GetType().GetProperty("UpdatedAt")?.SetValue(entry.Entity, DateTime.UtcNow);
                            }
                        }
                        break;


                    case EntityState.Deleted:
                        
                        entry.State = EntityState.Modified;
                        entry.Entity.IsDeleted = true;
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
