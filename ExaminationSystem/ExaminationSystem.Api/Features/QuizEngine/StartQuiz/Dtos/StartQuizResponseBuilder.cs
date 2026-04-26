using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public static class StartQuizResponseBuilder
    {
      
        public static Result<StartQuizResponse> Build(
            Result<QuizDto> quiz,
            Result<QuizAttemptDto> attempt,
            Result<ShuffleResult> shuffleResult,
            IReadOnlyList<StartQuizQuestionDto> originalQuestions)
        {
            var questionsDict = originalQuestions.ToDictionary(q => q.Id);

            var finalQuestions = shuffleResult.Value!.Questions
                .OrderBy(sq => sq.DisplayOrder)
                .Select(sq =>
                {
                    var originalQuestion = questionsDict[sq.QuestionId];
                    var optionsDict = originalQuestion.Options.ToDictionary(o => o.Id);

                    return new QuestionDto
                    {
                        QuestionId = sq.QuestionId,
                        Text = originalQuestion.Text,
                        DisplayOrder = sq.DisplayOrder,

                        Options = sq.Options
                            .OrderBy(so => so.DisplayOrder)
                            .Select(so =>
                            {
                                var originalOption = optionsDict[so.OptionId];


                                return new OptionDto
                                {
                                    OptionId = so.OptionId,
                                    Text = originalOption.Text,
                                    DisplayOrder = so.DisplayOrder,
                                };
                            }).ToList()
                    };
                }).ToList();

            var response = new StartQuizResponse
            {
                AttemptId = attempt.Value!.AttemptId,
                QuizId = quiz.Value!.Id,
                Title = quiz.Value.Title,
                DurationMinutes = quiz.Value.DurationMinutes,
                StartedAt = attempt.Value.StartedAt,
                Deadline = attempt.Value.Deadline,
                TotalQuestions = finalQuestions.Count,
                Questions = finalQuestions
            };

            return Result<StartQuizResponse>.Success(response);
        }
    }
}
