using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using MediatR;

namespace ExaminationSystem.Api.Features.Student_DashboardLearning.ViewQuizzesByDiploma
{
    public record CheckStudentEnrollmentQuery(Guid StudentId, Guid DiplomaId) : IRequest<bool>;
    public class CheckStudentEnrollmentHandler : IRequestHandler<CheckStudentEnrollmentQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CheckStudentEnrollmentHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<bool> Handle(CheckStudentEnrollmentQuery request, CancellationToken ct) =>
            await _unitOfWork.Repository<Enrollment, int>()
                .AnyAsync(e => e.StudentId == request.StudentId && e.DiplomaId == request.DiplomaId, ct);
    }
}
