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

            var perQuestion = answersDetail.Questions
                .Select(question =>
                {
                    var studentAnswer = answersDetail.Answers
                        .FirstOrDefault(a => a.QuestionId == question.Id);

                    var correctOption = answersDetail.Options
                        .First(o => o.QuestionId == question.Id && o.IsCorrect);

                    var selectedOption = studentAnswer is not null
                        ? answersDetail.Options
                            .FirstOrDefault(o => o.Id == studentAnswer.SelectedOptionId)
                        : null;

                    return new QuestionResultDto
                    {
                        QuestionId = question.Id,
                        Text = question.Text,
                        Explanation = question.Explanation,
                        StudentAnswerId = selectedOption?.Id ?? Guid.Empty,
                        StudentAnswerText = selectedOption?.Text ?? "Not Answered",
                        CorrectAnswerId = correctOption.Id,
                        CorrectAnswerText = correctOption.Text,
                        IsCorrect = studentAnswer?.IsCorrect ?? false
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
                TotalQuestions = attempt.TotalQuestions ?? 0,
                CorrectCount = correctCount,            
                Status = attempt.Status,
                SubmittedAt = attempt.SubmittedAt,
                PerQuestion = perQuestion
            };
        }
    }
}

