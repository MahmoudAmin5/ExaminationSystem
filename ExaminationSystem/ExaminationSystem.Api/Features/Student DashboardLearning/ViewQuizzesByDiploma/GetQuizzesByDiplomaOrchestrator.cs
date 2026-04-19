using ExaminationSystem.Api.BuildingBlocks.Interfaces;
using ExaminationSystem.Api.Shared.Results;
using MediatR;

namespace ExaminationSystem.Api.Features.Student_DashboardLearning.ViewQuizzesByDiploma
{
    public record GetQuizzesByDiplomaQuery(Guid DiplomaId) : IRequest<Result<List<QuizForStudentDto>>>;

    public class GetQuizzesByDiplomaOrchestrator : IRequestHandler<GetQuizzesByDiplomaQuery, Result<List<QuizForStudentDto>>>
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUser;

        public GetQuizzesByDiplomaOrchestrator(IMediator mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        public async Task<Result<List<QuizForStudentDto>>> Handle(GetQuizzesByDiplomaQuery request, CancellationToken ct)
        {
            var studentId = _currentUser.UserId ?? Guid.Empty;

            var diplomaResult = await _mediator.Send(new GetPublishedDiplomaByIdQuery(request.DiplomaId), ct);

            if (diplomaResult.IsFailure) 
                return Result<List<QuizForStudentDto>>.Failure(diplomaResult.Errors);

            
            var isEnrolled = await _mediator.Send(new CheckStudentEnrollment(studentId, request.DiplomaId), ct);

            if (!isEnrolled)
                return Result<List<QuizForStudentDto>>.Failure(Error.Forbidden("Access.Denied", "You are not enrolled in this diploma."));

            var quizzes = await _mediator.Send(new GetQuizzesWithHistoryQuery(request.DiplomaId, studentId), ct);

            return Result<List<QuizForStudentDto>>.Success(quizzes);
        }
    }

}
