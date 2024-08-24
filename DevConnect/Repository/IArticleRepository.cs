using DevConnect.Models;

namespace DevConnect.Repository
{
    public interface IArticleRepository
    {
        Article AddArticle(Article article);
        Article? GetArticleById(int id);
        List<Article>? GetArticlesByAuthorId(int id);
        List<Article>? GetArticlesByTitle(string title);
        List<Article>? GetArticlesByCreatedDate(DateOnly date);
        List<Article>? GetNewestArticles(int quantity);
        bool RemoveArticleById(int id);
        bool RemoveArticlesByAuthorId(int id);

        Task<Article> AddArticleAsync(Article article);
        Task<Article?> GetArticleByIdAsync(int id);
        Task<List<Article>?> GetArticlesByAuthorIdAsync(int id);
        Task<List<Article>?> GetArticlesByTitleAsync(string title);
        Task<List<Article>?> GetArticlesByCreatedDateAsync(DateOnly date);
        Task<List<Article>?> GetNewestArticlesAsync(int quantity);
        Task<bool> RemoveArticleByIdAsync(int id);
        Task<bool> RemoveArticlesByAuthorIdAsync(int id);
    }
}
