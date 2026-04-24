using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.Student_DashboardLearning.ViewDiplomas.DiplomasQueries
{
    public record GetStudentPassedQuizIdsQuery(Guid StudentId) : IRequest<List<Guid>>;

    public class GetStudentPassedQuizIdsHandler : IRequestHandler<GetStudentPassedQuizIdsQuery, List<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetStudentPassedQuizIdsHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<List<Guid>> Handle(GetStudentPassedQuizIdsQuery request, CancellationToken ct)
        {
            return await _unitOfWork.Repository<QuizAttempt, Guid>().AsNoTracking()
                .Where(a => a.StudentId == request.StudentId && a.Passed == true)
                .Select(a => a.QuizId)
                .Distinct()
                .ToListAsync<Guid>(ct);
        }
    }
}
