namespace ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions.Commands
{
    public record CreateOptionDto
    (
        string Text,
        bool IsCorrect
    );

}
