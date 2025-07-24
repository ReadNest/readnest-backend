using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ReadNest.Application.Services;
using ReadNest.Infrastructure.Options;
using ReadNest.Shared.Common;

namespace ReadNest.Infrastructure.Services
{
    public class BookCoverService : IBookCoverService
    {
        private readonly HttpClient _httpClient;

        public BookCoverService(IHttpClientFactory httpClientFactory, IOptions<GeminiOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient(options.Value.GoogleBooks);
        }


        public async Task<BookSearchResult> GetBookInfoAsync(string title, string author)
        {
            string query = $"volumes?q=intitle:{title}+inauthor:{author}";
            var response = await _httpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode) return null;

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(json);

            if (result?.items != null && result.items.Count > 0)
            {
                var volume = result.items[0].volumeInfo;
                return new BookSearchResult
                {
                    Title = volume.title,
                    Author = volume.authors != null ? string.Join(", ", volume.authors.ToObject<List<string>>()) : "Unknown",
                    Thumbnail = volume.imageLinks?.thumbnail ?? "https://via.placeholder.com/150",
                    InfoLink = volume.infoLink ?? ""
                };
            }

            return null;
        }
    }
}
