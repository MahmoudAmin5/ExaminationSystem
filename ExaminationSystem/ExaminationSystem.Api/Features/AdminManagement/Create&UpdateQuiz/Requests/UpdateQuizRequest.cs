using ExaminationSystem.Api.Domain.Enums;

namespace ExaminationSystem.Api.Features.AdminManagement.Create_UpdateQuiz.Requests
{
    public record UpdateQuizRequest(
    string Title,
    Guid DiplomaId,
    int DurationMinutes,
    decimal PassScore,
    int MaxAttempts,
    string Instructions,
    ContentStatus Status
);
}
