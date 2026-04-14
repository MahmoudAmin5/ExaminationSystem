using ExaminationSystem.Api.Domain.Entities;

namespace ExaminationSystem.Api.Domain.Contracts.Repository.Contract
{
    public interface IUnitOfWork : IDisposable
    {
       

        IGenericRepository<T, TId> Repository<T, TId>()
            where T : BaseEntity<TId>
            where TId : notnull;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }

}
