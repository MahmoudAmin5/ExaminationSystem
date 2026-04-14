using ExaminationSystem.Api.Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExaminationSystem.Api.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasIndex(r => r.UserId).HasFilter("[IsRevoked] = 0 AND [IsUsed] = 0");
            builder.HasOne(r => r.User)            
             .WithMany(u => u.RefreshTokens)  
             .HasForeignKey(r => r.UserId)    
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
