namespace DevConnect.Services
{
    public class SearchApiService
    {
        private readonly HttpClient _httpClient;
        public SearchApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddQuestionAsync(int id, string title)
        {
            var question = new { Id = id, Title = title };
            var response = await _httpClient.PostAsync($"questions", JsonContent.Create(question));
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<List<int>?> GetSimilarQuestionsAsync(int id)
        {
            var response = await _httpClient.GetAsync($"questions/similar/{id}");
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<int>>();
            }
            return null;
        }

        public async Task<List<int>?> SearchQuestionsAsync(string title)
        {
            var response = await _httpClient.GetAsync($"questions/bytitle/{title}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<int>>();
            }
            return null;
        }

        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"questions/{id}");
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }


        public async Task<bool> AddArticleAsync(int id, string title)
        {
            var question = new { Id = id, Title = title };
            var response = await _httpClient.PostAsync("articles", JsonContent.Create(question));
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        public async Task<List<int>?> GetSimilarArticlesAsync(int id)
        {
            var response = await _httpClient.GetAsync($"articles/similar/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<int>>();
            }
            return null;
        }

        public async Task<List<int>?> SearchArticlesAsync(string title)
        {
            var response = await _httpClient.GetAsync($"articles/bytitle/{title}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<int>>();
            }
            return null;
        }

        public async Task<bool> DeleteArticleAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"articles/{id}");
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
    }
}
