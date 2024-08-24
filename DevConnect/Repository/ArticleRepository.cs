using DevConnect.Data;
using DevConnect.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevConnect.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        BaseDbContext _context;
        public ArticleRepository(BaseDbContext context)
        {
            _context = context;
        }
        public Article AddArticle(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
            article.Author = _context.Users.Find(article.AuthorId)!;
            return article;
        }

        public async Task<Article> AddArticleAsync(Article article)
        {
            await _context.Articles.AddAsync(article);
            await _context.SaveChangesAsync();
            article.Author = await _context.Users.FindAsync(article.AuthorId);
            return article;
        }

        public Article? GetArticleById(int id)
        {
            Article? article = _context.Articles.Find(id);
            if(article!=null)
            {
                article.Author = _context.Users.Find(article.AuthorId)!;
            }
            return article;
        }

        public async Task<Article?> GetArticleByIdAsync(int id)
        {
            Article? article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                article.Author = await _context.Users.FindAsync(article.AuthorId);
            }
            return article;
        }

        public List<Article>? GetArticlesByAuthorId(int id)
        {
            List<Article>? articles = _context.Articles.Where(x => x.AuthorId == id).ToList();
            if(articles!=null)
            {
                User author = _context.Users.Find(id)!;
                foreach (var article in articles)
                {
                    article.Author = author;
                }
            }
            return articles;
        }

        public async Task<List<Article>?> GetArticlesByAuthorIdAsync(int id)
        {
            List<Article>? articles = await _context.Articles.Where(x => x.AuthorId == id).ToListAsync();
            if (articles != null)
            {
                User author = await _context.Users.FindAsync(id);
                foreach (var article in articles)
                {
                    article.Author = author;
                }
            }
            return articles;
        }

        public List<Article>? GetArticlesByCreatedDate(DateOnly date)
        {
            List<Article>? articles = _context.Articles.Where(x => x.CreatedAt == date).ToList();
            if(articles!=null)
            {
                foreach (var article in articles)
                {
                    article.Author = _context.Users.Find(article.AuthorId)!;
                }
            }
            return articles;
        }

        public async Task<List<Article>?> GetArticlesByCreatedDateAsync(DateOnly date)
        {
            List<Article>? articles = await _context.Articles.Where(x => x.CreatedAt == date).ToListAsync();
            if (articles != null)
            {
                foreach (var article in articles)
                {
                    article.Author = await _context.Users.FindAsync(article.AuthorId);
                }
            }
            return articles;
        }

        public List<Article>? GetArticlesByTitle(string title)
        {
            List<Article>? articles = _context.Articles.Where(x => x.Title == title).ToList();
            if (articles != null)
            {
                foreach (var article in articles)
                {
                    article.Author = _context.Users.Find(article.AuthorId)!;
                }
            }
            return articles;
        }

        public async Task<List<Article>?> GetArticlesByTitleAsync(string title)
        {
            List<Article>? articles = await _context.Articles.Where(x => x.Title == title).ToListAsync();
            if (articles != null)
            {
                foreach (var article in articles)
                {
                    article.Author = await _context.Users.FindAsync(article.AuthorId);
                }
            }
            return articles;
        }

        public bool RemoveArticleById(int id)
        {
            Article? article = _context.Articles.Find(id);
            if(article!=null)
            {
                _context.Articles.Remove(article);
                _context.Comments.RemoveRange(_context.Comments.Where(x => x.ArticleId == id).ToList());
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveArticleByIdAsync(int id)
        {
            Article? article = await _context.Articles.FindAsync(id);
            if(article!=null)
            {
                _context.Articles.Remove(article);
                _context.Comments.RemoveRange(await _context.Comments.Where(x => x.ArticleId == id).ToListAsync());
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool RemoveArticlesByAuthorId(int id)
        {
            List<Article>? articles = _context.Articles.Where(x => x.AuthorId == id).ToList();
            if(articles!=null)
            {
                foreach (var article in articles)
                {
                    List<Comment>? comments = _context.Comments.Where(x => x.ArticleId == article.Id).ToList();
                    if(comments!=null)
                    {
                        _context.Comments.RemoveRange(comments);
                    }
                }
                _context.Articles.RemoveRange(articles);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveArticlesByAuthorIdAsync(int id)
        {
            List<Article>? articles = await _context.Articles.Where(x => x.AuthorId == id).ToListAsync();
            if(articles!=null)
            {
                foreach (var article in articles)
                {
                    List<Comment>? comments = await _context.Comments.Where(x => x.ArticleId == article.Id).ToListAsync();
                    if (comments != null)
                    {
                        _context.Comments.RemoveRange(comments);
                    }
                }
                _context.Articles.RemoveRange(articles);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public List<Article>? GetNewestArticles(int quantity)
        {
            List<Article>? articles = _context.Articles.OrderByDescending(x => x.Id).Take(quantity).ToList();
            if(articles!=null)
            {
                foreach (var article in articles)
                {
                    article.Author = _context.Users.Find(article.AuthorId)!;
                }
            }
            return articles;
        }

        public async Task<List<Article>?> GetNewestArticlesAsync(int quantity)
        {
            List<Article>? articles = await _context.Articles.OrderByDescending(x => x.Id).Take(quantity).ToListAsync();
            if (articles != null)
            {
                foreach (var article in articles)
                {
                    article.Author = await _context.Users.FindAsync(article.AuthorId);
                }
            }
            return articles;
        }
    }
}
