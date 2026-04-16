using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Shared.Results;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public static class StartQuizResponseBuilder
    {
        public static StartQuizResponse Build(
            Result<QuizDto> quiz,
            Result<QuizAttemptDto> attempt,
            Result<ShuffleResult> shuffleResult,
            IReadOnlyList<Question> originalQuestions,
            IReadOnlyList<AnswerOption> originalOptions)
        {
            return new StartQuizResponse
            {
                AttemptId = attempt.Value.AttemptId,
                QuizId = quiz.Value.Id,
                Title = quiz.Value.Title,
                DurationMinutes = quiz.Value.DurationMinutes,
                StartedAt = attempt.Value.StartedAt,
                Deadline = attempt.Value.Deadline,
                TotalQuestions = shuffleResult.Value.Questions.Count,
                Questions = shuffleResult.Value.Questions
                    .OrderBy(sq => sq.DisplayOrder)
                    .Select(sq =>
                    {
                        var originalQuestion = originalQuestions
                            .First(q => q.Id == sq.QuestionId);

                        return new QuestionDto
                        {
                            QuestionId = sq.QuestionId,
                            Text = originalQuestion.Text,
                            Options = sq.Options
                                .OrderBy(so => so.DisplayOrder)
                                .Select(so =>
                                {
                                    var originalOption = originalOptions
                                        .First(o => o.Id == so.OptionId);

                                    return new OptionDto
                                    {
                                        OptionId = so.OptionId,
                                        Text = originalOption.Text
 
                                    };
                                }).ToList()
                        };
                    }).ToList()
            };
        }
    }
}
