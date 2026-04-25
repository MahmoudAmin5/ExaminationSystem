namespace ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion
{
    public record UpdateOptionDto
        (Guid? Id,
        string Text,
        bool IsCorrect);

}
