namespace ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.Dtos
{
    public record AttemptListDto(
    Guid AttemptId,
    Guid StudentId,
    string StudentName,
    string QuizTitle,   
    decimal? Score,
    string Status,
    DateTime? SubmittedAt
);
}
