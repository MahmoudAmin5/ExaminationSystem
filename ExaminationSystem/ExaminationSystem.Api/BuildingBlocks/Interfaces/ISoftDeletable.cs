namespace ExaminationSystem.Api.BuildingBlocks.Interfaces
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
