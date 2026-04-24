namespace ExaminationSystem.Api.Features.Student_DashboardLearning.ViewDiplomas
{
    public class DiplomaViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int QuizCount { get; set; }
        public double StudentProgress { get; set; }
    }
}
