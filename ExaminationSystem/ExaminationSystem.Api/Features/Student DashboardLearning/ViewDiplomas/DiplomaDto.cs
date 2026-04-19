namespace ExaminationSystem.Api.Features.Diplomas.ViewDiplomas
{
    public record DiplomaDto(
      Guid Id,
    string Title,
    string Description,
     int QuizzesCount,
    double ProgressPercentage);

}
