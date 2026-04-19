using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.Student_DashboardLearning.ViewQuizzesByDiploma
{
    public record GetQuizzesWithHistoryQuery(Guid DiplomaId, Guid StudentId) : IRequest<List<QuizForStudentDto>>;

    public class GetQuizzesWithHistoryHandler : IRequestHandler<GetQuizzesWithHistoryQuery, List<QuizForStudentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetQuizzesWithHistoryHandler(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<List<QuizForStudentDto>> Handle(GetQuizzesWithHistoryQuery request, CancellationToken ct)
        {
           
            var quizQuery = _unitOfWork.Repository<Quiz, Guid>()
                .AsNoTracking()
                .Where(q => q.DiplomaId == request.DiplomaId && q.Status == ContentStatus.Published);

        
            return await quizQuery.Select(q => new QuizForStudentDto(
                q.Id,
                q.Title,
                q.DurationMinutes,

                q.Attempts.Count(a => a.StudentId == request.StudentId),

                q.Attempts.Where(a => a.StudentId == request.StudentId)
                          .OrderByDescending(a => a.StartedAt)
                          .Select(a => a.Score)
                          .FirstOrDefault(),

                q.Attempts.Where(a => a.StudentId == request.StudentId)
                          .OrderByDescending(a => a.StartedAt)
                          .Select(a => a.Status.ToString())
                          .FirstOrDefault() ?? "Available"
            ))
           
            .ToListAsync<QuizForStudentDto>(ct);
        }
    }
}
