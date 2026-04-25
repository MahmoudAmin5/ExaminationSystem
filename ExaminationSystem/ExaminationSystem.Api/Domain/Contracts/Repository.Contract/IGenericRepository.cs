using ExaminationSystem.Api.Domain.Entities;
using System.Linq.Expressions;

namespace ExaminationSystem.Api.Domain.Contracts.Repository.Contract
{
    public interface IGenericRepository<T, TId> where T : BaseEntity<TId> where TId : notnull


    {

        Task<T?> GetByIdAsync(TId id, CancellationToken cancellationToken = default);


        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);


        Task<IReadOnlyList<T>> FindAsync( Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<T?> FirstOrDefaultAsync( Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync( Expression<Func<T, bool>> predicate,CancellationToken cancellationToken = default);

        Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null,CancellationToken cancellationToken = default);
        IQueryable<T> AsNoTracking();

        void Add(T entity);
        void AddRange(IEnumerable<T> entities);

        void SaveInclude(T entity, params string[] includedProperties);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}
