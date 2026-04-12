using ExaminationSystem.Api.BuildingBlocks.Interfaces;

namespace ExaminationSystem.Api.Domain.Entities
{
    
    public abstract class BaseEntity<TKey> : ISoftDeletable
    {
        public TKey Id { get; set; } = default!; 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

       
        public bool IsDeleted { get; set; } = false; 
        public DateTime? DeletedAt { get; set; }
    }
}
