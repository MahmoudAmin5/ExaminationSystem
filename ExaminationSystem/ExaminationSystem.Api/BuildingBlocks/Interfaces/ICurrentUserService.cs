namespace ExaminationSystem.Api.BuildingBlocks.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
    }
}
