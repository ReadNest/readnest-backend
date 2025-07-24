using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace ReadNest.Infrastructure.Services
{
    public class GeminiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;

        public GeminiClient(string apiKey, string baseUrl, string model = "gemini-1.5-flash-latest")
        {
            _model = model;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        }

        public async Task<string> AskAsync(string prompt)
        {
            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var response = await _httpClient.PostAsync(
                $"models/{_model}:generateContent",
                new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
            );

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            dynamic result = JsonConvert.DeserializeObject(json);
            return (string)result.candidates[0].content.parts[0].text;
        }
    }
}
