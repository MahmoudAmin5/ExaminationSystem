namespace ExaminationSystem.Api.Features.Diplomas.Dtos
{
    public record DiplomaDto(
      Guid Id,
    string Title,
    string Description,
     int QuizzesCount,
    double ProgressPercentage);

}
