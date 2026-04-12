using ExaminationSystem.Api.Domain.Entities.Account;
using ExaminationSystem.Api.Domain.Entities.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Configurations
{
    public class PasswordResetTokenConfigurationbuilder: IEntityTypeConfiguration<PasswordResetToken>
    {
        public void Configure(EntityTypeBuilder<PasswordResetToken> builder)
        {

            builder.HasOne(r => r.User)
           .WithMany(u => u.PasswordResetTokens)
           .HasForeignKey(r => r.UserId)
           .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
