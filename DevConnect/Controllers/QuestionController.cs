using DevConnect.Models;
using DevConnect.Repository;
using DevConnect.Services;
using DevConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private const string _validationOk = "ok";
        IQuestionRepository _questionRepository;
        IUserRepository _userRepository;
        ICommentRepository _commentRepository;
        SearchApiService _apiService;
        public QuestionController(IUserRepository userRepository, ICommentRepository commentRepository,
            IQuestionRepository questionRepository, SearchApiService apiService)
        {
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _apiService = apiService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(Question question)
        {
            question.Author = await _userRepository.GetUserByIdAsync(question.AuthorId);
            QuestionCommentsViewModel questionComments = new QuestionCommentsViewModel();
            questionComments.CurrentQuestion = question;
            questionComments.Comments = await _commentRepository.GetCommentsByQuestionIdAsync(question.Id);
            if (User.Identity.Name == question.Author.Email)
            {
                ViewBag.Delete = true;
            }
            return View(questionComments);
        }

        [HttpGet]
        public IActionResult CreateQuestion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateQuestion(Question question)
        {
            string validateResult = ValidateQuestion(question);
            if (validateResult != _validationOk)
            {
                ViewBag.ErrorMEssage = validateResult;
                return View(question);
            }
            User author = await _userRepository.GetUserByEmailAsync(User.Identity.Name);
            question.AuthorId = author!.Id;
            question.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            question.Id = default;
            question = await _questionRepository.AddQuestionAsync(question);
            await _apiService.AddQuestionAsync(question.Id, question.Title);
            return RedirectToAction("Index", question);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            if (User.Identity.Name == _questionRepository.GetQuestionById(id).Author.Email)
            {
                await _questionRepository.RemoveQuestionByIdAsync(id);
                await _apiService.DeleteQuestionAsync(id);
            }
            return RedirectToAction("Index", "User");
        }

        private string ValidateQuestion(Question question)
        {
            if (string.IsNullOrWhiteSpace(question.Title))
            {
                return "Enter the question title.";
            }
            if (string.IsNullOrWhiteSpace(question.Content))
            {
                return "Write the question.";
            }
            return _validationOk;
        }
    }
}
