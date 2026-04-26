namespace ExaminationSystem.Api.Features.QuizEngine.TimerHandling.Dtos
{
    public class AttemptTimerDto
    {
        public Guid AttemptId { get; set; }
        public int SecondsRemaining { get; set; }
        public DateTime Deadline { get; init; }
        public bool IsExpired { get; init; }
    }
}
