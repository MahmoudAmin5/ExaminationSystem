namespace ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Dtos
{
    public class AttemptTimerDto
    {
        public Guid AttemptId { get; set; }
        public DateTime SecondsReamning { get; set; }
        public DateTime Deadline { get; init; }
        public bool IsExpired { get; init; }
    }
}
