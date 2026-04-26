namespace ExaminationSystem.Api.Features.QuizEngine.ViewResult.Dtos
{
    public class ViewResultsResponseBuilder
    {
        public static AttemptResultDto Build(
            AttemptWithQuizDto attemptWithQuiz,
            AttemptAnswersDetailDto answersDetail) 
        {
            var attempt = attemptWithQuiz.Attempt;
            var quiz = attemptWithQuiz.Quiz;

            var perQuestion = answersDetail.ReviewedQuestions
                .Select(question =>
                {
                   
                    var correctOption = question.Options.First(o => o.IsCorrect);

                   
                    var selectedOption = question.StudentAnswer?.SelectedOptionId != null
                        ? question.Options.FirstOrDefault(o => o.OptionId == question.StudentAnswer.SelectedOptionId)
                        : null;

                    return new QuestionResultDto
                    {
                        QuestionId = question.QuestionId,
                        Text = question.Text,
                        Explanation = question.Explanation,

                     
                        StudentAnswerId = selectedOption?.OptionId ?? Guid.Empty,
                        StudentAnswerText = selectedOption?.Text ?? "Not Answered",

                        CorrectAnswerId = correctOption.OptionId,
                        CorrectAnswerText = correctOption.Text,
                        IsCorrect = question.StudentAnswer?.IsCorrect ?? false
                    };
                }).ToList();

            var correctCount = perQuestion.Count(q => q.IsCorrect);

            return new AttemptResultDto
            {
                AttemptId = attempt.Id,
                QuizId = quiz.Id,
                QuizTitle = quiz.Title,
                Score = attempt.Score ?? 0,
                Passed = attempt.Passed ?? false,
                CorrectCount = correctCount,
                Status = attempt.Status,
                SubmittedAt = attempt.SubmittedAt,
                PerQuestion = perQuestion
            };
        }
    }
}
