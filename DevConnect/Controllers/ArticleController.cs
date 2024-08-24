using DevConnect.Models;
using DevConnect.Repository;
using DevConnect.Services;
using DevConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Controllers
{
    [Authorize]
    public class ArticleController : Controller
    {
        private const string _validationOk = "ok";
        IArticleRepository _articleRepository;
        IUserRepository _userRepository;
        ICommentRepository _commentRepository;
        SearchApiService _apiService;
        public ArticleController(IArticleRepository articleRepository, 
            IUserRepository userRepository, ICommentRepository commentRepository, SearchApiService apiService)
        {
            _articleRepository = articleRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Article article)
        {
            article.Author = await _userRepository.GetUserByIdAsync(article.AuthorId);
            ArticleCommentsViewModel articleComments = new ArticleCommentsViewModel();
            articleComments.CurrentArticle = article;
            articleComments.Comments = await _commentRepository.GetCommentsByArticleIdAsync(article.Id);
            if(User.Identity.Name==article.Author.Email)
            {
                ViewBag.Delete = true;
            }
            return View(articleComments);
        }

        [HttpGet]
        public IActionResult CreateArticle()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateArticle(Article article)
        {
            string validateResult = ValidateArticle(article);
            if(validateResult!=_validationOk)
            {
                ViewBag.ErrorMEssage = validateResult;
                return View(article);
            }
            User author = await _userRepository.GetUserByEmailAsync(User.Identity.Name);
            article.AuthorId = author!.Id;
            article.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            article.Id = default;
            article = await _articleRepository.AddArticleAsync(article);
            await _apiService.AddArticleAsync(article.Id, article.Title);
            return RedirectToAction("Index", article);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if(User.Identity.Name == _articleRepository.GetArticleById(id).Author.Email)
            {
                await _articleRepository.RemoveArticleByIdAsync(id);
                await _apiService.DeleteArticleAsync(id);
            }
            return RedirectToAction("Index", "User");
        }

        private string ValidateArticle(Article article)
        {
            if(string.IsNullOrWhiteSpace(article.Title))
            {
                return "Enter the article title.";
            }
            if(string.IsNullOrWhiteSpace(article.Content))
            {
                return "Write the article.";
            }
            return _validationOk;
        }
    }
}
