using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.Student_DashboardLearning.ViewQuizzesByDiploma
{

    public record GetPublishedDiplomaByIdQuery(Guid DiplomaId) : IRequest<Result<Diploma>>;

    public class GetPublishedDiplomaByIdHandler : IRequestHandler<GetPublishedDiplomaByIdQuery, Result<Diploma>>

    {
        private readonly IUnitOfWork _unitOfWork;
        public GetPublishedDiplomaByIdHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<Result<Diploma>> Handle(GetPublishedDiplomaByIdQuery request, CancellationToken ct)
        {
            var diploma = await _unitOfWork.Repository<Diploma, Guid>()
                .FirstOrDefaultAsync(d => d.Id == request.DiplomaId && d.Status == ContentStatus.Published, ct);

            return diploma == null
                ? Result<Diploma>.Failure(Error.NotFound("Diploma.NotFound", "Diploma not found or not published."))
                : Result<Diploma>.Success(diploma);

        }
    }
}
