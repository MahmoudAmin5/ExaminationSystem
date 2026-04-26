using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos;
using ExaminationSystem.Api.Infrastructure.Persistence;
using ExaminationSystem.Api.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Commands
{
    public record SaveShuffledQuestionsOrderCommand(
        Guid AttemptId,
        IReadOnlyList<StartQuizQuestionDto> Questions) : IRequest<Result<ShuffleResult>>;

    public class SaveShuffledQuestionsOrderCommandHandler : IRequestHandler<SaveShuffledQuestionsOrderCommand, Result<ShuffleResult>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SaveShuffledQuestionsOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<Result<ShuffleResult>> Handle(SaveShuffledQuestionsOrderCommand request, CancellationToken cancellationToken)
        {
            var shuffledQuestions = Shuffle(request.Questions.ToList());

            var questionOrders = new List<AttemptQuestionOrder>();
            var optionOrders = new List<AttemptOptionOrder>();
            var resultQuestions = new List<ShuffledQuestionDto>();

            for (var i = 0; i < shuffledQuestions.Count; i++)
            {
                var question = shuffledQuestions[i];

                questionOrders.Add(new AttemptQuestionOrder
                {
                    AttemptId = request.AttemptId,
                    QuestionId = question.Id,
                    DisplayOrder = i
                });

                var shuffledOptions = Shuffle(question.Options.ToList());

                var resultOptions = new List<ShuffledOptionDto>();

                for (var j = 0; j < shuffledOptions.Count; j++)
                {
                    optionOrders.Add(new AttemptOptionOrder
                    {
                        AttemptId = request.AttemptId,
                        OptionId = shuffledOptions[j].Id,
                        DisplayOrder = j
                    });

                    resultOptions.Add(new ShuffledOptionDto
                    {
                        OptionId = shuffledOptions[j].Id,
                        DisplayOrder = j
                    });
                }

                resultQuestions.Add(new ShuffledQuestionDto
                {
                    QuestionId = question.Id,
                    DisplayOrder = i,
                    Options = resultOptions
                });
            }
            _unitOfWork.QuizAttempts.SaveShuffledOrders(questionOrders, optionOrders);

            return Task.FromResult(Result<ShuffleResult>.Success(new ShuffleResult
            {
                Questions = resultQuestions
            }));
        }

        private static List<T> Shuffle<T>(List<T> list)
        {
            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = Random.Shared.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }

            return list;
        }
    }
}
