namespace ExaminationSystem.Api.Features.Student_DashboardLearning.ViewQuizzesByDiploma
{
    public record QuizForStudentDto(
    Guid Id,
    string Title,
    int DurationMinutes,
    int AttemptCount,
    decimal? LastScore,
    string Status  
    );


}

