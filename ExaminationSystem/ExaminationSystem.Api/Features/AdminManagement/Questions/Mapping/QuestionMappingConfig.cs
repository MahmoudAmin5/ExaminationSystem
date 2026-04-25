using ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions;
using Mapster;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.Mapping
{
    public class QuestionMappingConfig :IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
          
            config.NewConfig<CreateQuestionViewModel, AddQuestionOrchestrator>();
            config.NewConfig<CreateOptionViewModel, CreateOptionDto>();

           
        }
    }
}
