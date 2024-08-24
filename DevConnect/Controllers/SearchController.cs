using DevConnect.Models;
using DevConnect.Models.Enums;
using DevConnect.Repository;
using DevConnect.Services;
using DevConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace DevConnect.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        IArticleRepository _articleRepository;
        IQuestionRepository _questionRepository;
        IUserRepository _userRepository;
        SearchApiService _searchApi;

        public SearchController(IArticleRepository articleRepository, IUserRepository userRepository,
            IQuestionRepository questionRepository, SearchApiService searchApi)
        {
            _articleRepository = articleRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _searchApi = searchApi;
        }

        [HttpGet]
        public async Task<IActionResult> Index(List<int> arrId, ModelTypeEnum type)
        {
            if (arrId.IsNullOrEmpty())
                return View();
            SearchViewModel model = new SearchViewModel();
            model.Type = type;
            if (type==ModelTypeEnum.articles)
            {
                model.Articles = new List<Article>();
                foreach (var id in arrId)
                {
                    model.Articles.Add(await _articleRepository.GetArticleByIdAsync(id));
                }
            }
            if (type == ModelTypeEnum.questions)
            {
                model.Questions = new List<Question>();
                foreach (var id in arrId)
                {
                    model.Questions.Add(await _questionRepository.GetQuestionByIdAsync(id));
                }
            }
            if (type == ModelTypeEnum.users)
            {
                model.FoundUser = await _userRepository.GetUserByIdAsync(arrId[0]);
                if (model.FoundUser == null)
                    return View(null);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Search(ModelTypeEnum type, string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return RedirectToAction("Index");

            List<int> arrId;
            if(type==ModelTypeEnum.articles)
            {
                arrId = await _searchApi.SearchArticlesAsync(query);
                if (arrId == null)
                    return RedirectToAction("Index");
                return RedirectToAction("Index", new {arrId=arrId, type=type});
            }
            if (type == ModelTypeEnum.questions)
            {
                arrId = await _searchApi.SearchQuestionsAsync(query);
                if (arrId == null)
                    return RedirectToAction("Index");
                return RedirectToAction("Index", new { arrId = arrId, type = type });
            }
            if (type == ModelTypeEnum.users)
            {
                arrId = new List<int>();
                User? user = await _userRepository.GetUserByUsernameAsync(query);
                if (user == null)
                    return RedirectToAction("Index");
                arrId.Add(user.Id);
                return RedirectToAction("Index", new { arrId = arrId, type = type });
            }
            return RedirectToAction("Index");
        }
    }
}
