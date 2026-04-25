using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using MediatR;

namespace ExaminationSystem.Api.Features.AdminManagement.Shard.Queries
{
    public record GetQuestionCountForQuiz(Guid quizId) : IRequest<int>;
    public class GetQuestionCountForQuizHandler : IRequestHandler<GetQuestionCountForQuiz, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetQuestionCountForQuizHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(GetQuestionCountForQuiz request, CancellationToken cancellationToken)
        {
           return await _unitOfWork.Repository<Question,Guid>()
                .CountAsync(q => q.QuizId == request.quizId, cancellationToken);
        }
    }

}
