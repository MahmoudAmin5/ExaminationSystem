using ExaminationSystem.Api.Domain.Entities.Data;

namespace ExaminationSystem.Api.Domain.Contracts.Repository.Contract
{
    public interface IQuizAttemptRepository : IGenericRepository<QuizAttempt, Guid>
    {
        void SaveShuffledOrders(
            IEnumerable<AttemptQuestionOrder> questionOrders,
            IEnumerable<AttemptOptionOrder> optionOrders);
    }
}
