using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public class QuizDto
    {
        public Guid Id { get; set; }
        public Guid DiplomaId { get; set; }
        public string Title { get; set; }
        public int DurationMinutes { get; set; }
        public decimal PassScore { get; set; }
        public int? MaxAttempts { get; set; }
        public ContentStatus Status { get; set; }
    }
}
