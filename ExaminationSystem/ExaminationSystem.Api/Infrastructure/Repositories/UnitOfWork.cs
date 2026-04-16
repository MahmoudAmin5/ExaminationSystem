using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;

namespace ExaminationSystem.Api.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly ConcurrentDictionary<string, object> _repositories;
        private IQuizAttemptRepository _quizAttempts;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }

        public IGenericRepository<T, TId> Repository<T, TId>()
            where T : BaseEntity<TId>
            where TId : notnull
        {
            var typeName = typeof(T).Name;

            var repository = _repositories.GetOrAdd(typeName, _ =>
                new GenericRepository<T, TId>(_context));

            return (IGenericRepository<T, TId>)repository;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public IQuizAttemptRepository QuizAttempts =>
        _quizAttempts ??= new QuizAttemptRepository(_context);

        public void Dispose()
        {
           _context.Dispose();
        }
    }
}
