namespace ExaminationSystem.Api.Features.AdminManagement.ViewAllAttempts.Requests
{
    public record GetAttemptsQueryParameters(
    int Page = 1,
    int PerPage = 20,
    Guid? QuizId = null,
    Guid? StudentId = null,
    string SortBy = "submitted_at",
    string Order = "desc"
);
}
