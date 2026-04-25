namespace ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion
{
    public record UpdateQuestionViewModel (
    string Text,
    string? Explanation,
    int OrderIndex,
    List<UpdateOptionViewModel> Options);

   
}
