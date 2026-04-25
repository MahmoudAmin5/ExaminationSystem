namespace ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.ViewModels
{
    public class AttemptListViewModel
    {
        public Guid AttemptId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string QuizTitle { get; set; } = string.Empty;
        public decimal? Score { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? SubmittedAt { get; set; }
    }
}
