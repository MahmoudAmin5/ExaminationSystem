namespace ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions
{
    public record CreateQuestionViewModel(
    string Text,
    string? Explanation,
    int OrderIndex,
    List<CreateOptionViewModel> Options);
}
