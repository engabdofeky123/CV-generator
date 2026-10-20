using CV_Generator.ViewModels;

namespace CV_Generator.Services.Interfaces
{
    public interface IGeneratePromptService
    {
        public string GeneratePrompt(CVFormSubmit cv);
    }
}
