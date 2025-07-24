using Newtonsoft.Json;
using ReadNest.Application.Services;
using ReadNest.Shared.Common;

namespace ReadNest.Infrastructure.Services
{
    public class GeminiService : IGeminiService
    {
        private readonly GeminiClient _geminiClient;

        public GeminiService(GeminiClient geminiClient)
        {
            _geminiClient = geminiClient;
        }

        public async Task<List<BookSuggestion>> GetRecommendationsAsync(List<UserAnswer> answers)
        {
            var prompt = new PromptBuilder()
                .AddAnswerList(answers)
                .Build();

            var rawResponse = await _geminiClient.AskAsync(prompt);
            var cleanJson = CleanJson(rawResponse);

            return JsonConvert.DeserializeObject<List<BookSuggestion>>(cleanJson) ?? new List<BookSuggestion>();
        }

        private string CleanJson(string input)
        {
            return input.Replace("```json", "").Replace("```", "").Trim();
        }
    }
}
