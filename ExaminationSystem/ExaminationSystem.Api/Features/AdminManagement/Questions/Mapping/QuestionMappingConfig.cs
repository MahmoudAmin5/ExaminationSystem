using ExaminationSystem.Api.Features.AdminManagement.Questions.AddQuestions;
using ExaminationSystem.Api.Features.AdminManagement.Questions.UpdateQuestion;
using Mapster;

namespace ExaminationSystem.Api.Features.AdminManagement.Questions.Mapping
{
    public class QuestionMappingConfig :IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
          
            config.NewConfig<CreateQuestionViewModel, AddQuestionOrchestrator>();
            config.NewConfig<CreateOptionViewModel, CreateOptionDto>();
            config.NewConfig<UpdateQuestionViewModel, UpdateQuestionOrchestrator>();
            config.NewConfig<UpdateOptionViewModel, UpdateOptionDto>();


        }
    }
}
