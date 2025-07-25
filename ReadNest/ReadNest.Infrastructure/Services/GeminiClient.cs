using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace ReadNest.Infrastructure.Services
{
    public class GeminiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;
        private readonly string _apiKey;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="baseUrl"></param>
        /// <param name="model"></param>
        public GeminiClient(string apiKey, string baseUrl, string model = "gemini-1.5-flash-latest")
        {
            _apiKey = apiKey;
            _model = model;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
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

            var url = $"models/{_model}:generateContent?key={_apiKey}";

            var response = await _httpClient.PostAsync(
                url,
                new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
            );

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Gemini API Error ({response.StatusCode}): {error}");
            }

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);
            return (string)result.candidates[0].content.parts[0].text;
        }
    }
}
