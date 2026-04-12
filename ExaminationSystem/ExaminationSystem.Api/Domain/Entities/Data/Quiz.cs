using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class Quiz : BaseEntity<Guid>
    {
        public string Title { get; set; } = string.Empty;
        public Guid DiplomaId { get; set; }
        public int DurationMinutes { get; set; }
        public decimal PassScore { get; set; } 
        public int? MaxAttempts { get; set; }
        public ContentStatus Status { get; set; } = ContentStatus.Draft;
        public string? Instructions { get; set; }

        public DateTime? PublishedAt { get; set; }
        public virtual Diploma Diploma { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
}
