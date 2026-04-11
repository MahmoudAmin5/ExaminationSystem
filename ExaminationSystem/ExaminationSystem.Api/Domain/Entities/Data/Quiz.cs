using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Domain.Entities.Data
{
    public class Quiz : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public int DiplomaId { get; set; }
        public int DurationMinutes { get; set; }
        public int PassScore { get; set; } = 60;
        public int? MaxAttempts { get; set; }
        public ContentStatus Status { get; set; } = ContentStatus.Draft;
        public string? Instructions { get; set; }
        public int TotalAttemptsCount { get; set; }
        public double AveragePassRate { get; set; }
        public virtual Diploma Diploma { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
        public virtual ICollection<QuizAttempt> Attempts { get; set; } = new List<QuizAttempt>();
    }
}
