using DevConnect.Models;
using DevConnect.Repository;
using DevConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevConnect.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private const string _validationOk = "ok";
        ICommentRepository _commentRepository;
        IUserRepository _userRepository;
        IArticleRepository _articleRepository;
        IQuestionRepository _questionRepository;
        public CommentController(ICommentRepository commentRepository, IUserRepository userRepository,
            IQuestionRepository questionRepository, IArticleRepository articleRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _articleRepository = articleRepository;
            _questionRepository = questionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(CommentCreationViewModel commentCreation)
        {
            if(ValidateComment(commentCreation)==_validationOk)
            {
                Comment comment = commentCreation.CurrentComment;
                comment.AuthorId = _userRepository.GetUserByEmail(User.Identity.Name).Id;
                comment.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
                await _commentRepository.AddCommentAsync(comment);
                if (comment.ArticleId != null)
                    commentCreation.ArticleId = comment.ArticleId;
                else
                    commentCreation.QuestionId = comment.QuestionId;
            }
            if (commentCreation.ArticleId != null)
            {
                return RedirectToAction("Index", "Article", await _articleRepository.GetArticleByIdAsync((int)commentCreation.ArticleId));
            }
            return RedirectToAction("Index", "Question", await _questionRepository.GetQuestionByIdAsync((int)commentCreation.QuestionId!));
        }

        private string ValidateComment(CommentCreationViewModel commentCreation)
        {
            if(string.IsNullOrWhiteSpace(commentCreation.CurrentComment.Content))
            {
                return "Write the comment.";
            }
            return _validationOk;
        }
    }
}
