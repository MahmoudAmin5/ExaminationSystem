namespace ExaminationSystem.Api.Features.QuizEngine.StartQuiz.Dtos
{
    public class StartQuizQuestionsWithOptionsDto
    {
        public IReadOnlyList<StartQuizQuestionDto> Questions { get; set; } = new List<StartQuizQuestionDto>();
    }

    public class StartQuizQuestionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;

        public IReadOnlyList<StartQuizOptionDto> Options { get; set; } = new List<StartQuizOptionDto>();
    }

    public class StartQuizOptionDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; } = string.Empty;

    }
}
