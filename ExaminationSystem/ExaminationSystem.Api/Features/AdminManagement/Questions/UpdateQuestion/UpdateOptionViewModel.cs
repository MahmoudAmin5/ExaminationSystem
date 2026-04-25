namespace ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion
{
    public record UpdateOptionViewModel
        (Guid? Id, 
        string Text,
        bool IsCorrect);


}
