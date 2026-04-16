using ExaminationSystem.Api.Domain.Contracts.Repository.Contract;
using ExaminationSystem.Api.Domain.Entities.Data;
using ExaminationSystem.Api.Infrastructure.Persistence;

namespace ExaminationSystem.Api.Infrastructure.Repositories
{
    public class QuizAttemptRepository : GenericRepository<QuizAttempt, Guid>, IQuizAttemptRepository
    {
        private readonly ApplicationDbContext _context;

        public QuizAttemptRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void SaveShuffledOrders(
            IEnumerable<AttemptQuestionOrder> questionOrders,
            IEnumerable<AttemptOptionOrder> optionOrders)
        {
            _context.Set<AttemptQuestionOrder>().AddRange(questionOrders);
            _context.Set<AttemptOptionOrder>().AddRange(optionOrders);
        }
    }
}
