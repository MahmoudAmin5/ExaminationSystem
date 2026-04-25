using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.AdminManagement.Shard.Queries
{
    public record IsDiplomaExistQuery(Guid DiplomaId) : IRequest<bool>;
    public class CheckDiplomaExistenceHandler : IRequestHandler<IsDiplomaExistQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CheckDiplomaExistenceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> Handle(IsDiplomaExistQuery request, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Diploma, Guid>();
            return await repo.AsNoTracking().AnyAsync(d => d.Id == request.DiplomaId, cancellationToken);
        }
    }
}
