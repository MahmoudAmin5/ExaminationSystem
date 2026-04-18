namespace ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Dtos
{
    public class AutoSubmitDto
    {
        public Guid AttemptId { get; set; }
        public DateTime SubmittedAt { get; set; }
        public bool WasAutoSubmitted { get; set; }
        public decimal? Score { get; set; }
        public bool IsPassed { get; set; }
    }
}
