using ExaminationSystem.Api.Domain.Entities.Data;

namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public class QuizQuestionsDto
    {
        public IReadOnlyList<Question> Questions { get; set; }
        public IReadOnlyList<AnswerOption> Options { get; set; }
    }
}
