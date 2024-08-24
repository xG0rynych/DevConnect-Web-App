using DevConnect.Models;
using DevConnect.Models.Enums;
using DevConnect.Repository;
using DevConnect.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace DevConnect.Controllers
{
    public class UserController : Controller
    {
        private const string _validationOk = "ok";

        IUserRepository _userRepository;
        IArticleRepository _articleRepository;
        IQuestionRepository _questionRepository;
        ICommentRepository _commentRepository;
        IChatRepository _chatRepository;
        IMessageRepository _messageRepository;
        public UserController(
            IUserRepository userRepository, 
            IQuestionRepository questionRepository, 
            IArticleRepository articleRepository,
            ICommentRepository commentRepository,
            IChatRepository chatRepository,
            IMessageRepository messageRepository)
        {
            _userRepository = userRepository;
            _articleRepository = articleRepository;
            _questionRepository = questionRepository;
            _commentRepository = commentRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            User? user = await _userRepository.GetUserByEmailAsync(User.Identity?.Name);
            if (user == null)
                return RedirectToAction("ErrorView", "Home");
            UserIndexViewModel userIndex = new UserIndexViewModel();
            userIndex.CurrentUser = user;
            userIndex.Articles = await _articleRepository.GetNewestArticlesAsync(5);
            userIndex.Questions = await _questionRepository.GetNewestQuestionsAsync(5);
            return View(userIndex);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(string email)
        {
            User? user = await _userRepository.GetUserByEmailAsync(email);
            if(user==null)
            {
                return RedirectToAction("ErrorView", "Home");
            }
            ProfileViewModel profile = new ProfileViewModel();
            profile.CurrentUser = user;
            profile.Articles = await _articleRepository.GetArticlesByAuthorIdAsync(user.Id);
            profile.Questions = await _questionRepository.GetQuestionsByAuthorIdAsync(user.Id);
            if (User.Identity.Name == email)
            {
                ViewBag.Update = true;
                ViewBag.Chat = true;
            }
            List<Chat> chats = await _chatRepository.GetByUserAsync(user.Id);
            if(chats!=null)
            {
                ChatsUserViewModel chatsModel = new ChatsUserViewModel();
                chatsModel.ChatsId = new List<int>();
                chatsModel.Users = new List<string>();
                chatsModel.LastMessage = new List<string>();
                chatsModel.SendAt = new List<DateTime>();
                foreach (var chat in chats)
                {
                    if(user.Id!=chat.FirstUserId)
                    {
                        chatsModel.Users.Add((await _userRepository.GetUserByIdAsync(chat.FirstUserId)).Username);
                    }
                    else
                    {
                        chatsModel.Users.Add((await _userRepository.GetUserByIdAsync(chat.SecondUserId)).Username);
                    }
                    Message lastMessage = await _messageRepository.GetLastMessageByChatAsync(chat.Id);
                    chatsModel.ChatsId.Add(chat.Id);
                    if(lastMessage!=null)
                    {
                        chatsModel.LastMessage.Add(lastMessage.Content);
                        chatsModel.SendAt.Add(lastMessage.SendAt);
                    }
                    else
                    {
                        chatsModel.LastMessage.Add(null);
                        chatsModel.SendAt.Add(default);
                    }
                }
                ViewBag.Chats = chatsModel;
            }
            return View(profile);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(User.Identity!=null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel userLogin)
        {
            if(string.IsNullOrWhiteSpace(userLogin.Email))
            {
                ViewBag.ErrorMessage = "Enter the email.";
                return View(userLogin);
            }
            if(string.IsNullOrWhiteSpace(userLogin.Password))
            {
                ViewBag.ErrorMessage = "Enter the password.";
                return View(userLogin);
            }
            userLogin.Password = Hash(userLogin.Password);
            User? user = await _userRepository.LoginAsync(userLogin.Email, userLogin.Password);
            if(user==null)
            {
                ViewBag.ErrorMessage = "Wrong email or password.";
                return View(userLogin);
            }
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity));
            HttpContext.Session.SetString("name", user.Email);
            HttpContext.Session.SetString("role", user.Role.ToString());
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Registration()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(User user)
        {
            string validationResult = await ValidateUserAsync(user);
            if(validationResult!=_validationOk)
            {
                ViewBag.ErrorMessage = validationResult;
                return View(user);
            }
            user.Password = Hash(user.Password);
            user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            user.Role = RoleEnum.user.ToString();
            await _userRepository.AddUserAsync(user);
            return RedirectToAction("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            User? user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return RedirectToAction("Index");
            return View(new UserUpdateViewModel() { Bio = user.Bio, Skills = user.Skills});
        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateViewModel user)
        {
            User userDb = await _userRepository.GetUserByEmailAsync(User.Identity.Name);
            userDb.Bio = user.Bio;
            userDb.Skills = user.Skills;
            if (user.Avatar != null && user.Avatar.Length > 0)
            {
                var fileExtension = Path.GetExtension(user.Avatar.FileName);
                var uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine("wwwroot/images", uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await user.Avatar.CopyToAsync(stream);
                }
                userDb.ProfilePicture = $"/images/{uniqueFileName}";
            }
            await _userRepository.UpdateUserAsync(userDb.Id, userDb);
            return RedirectToAction("Index");
        }



        private async Task<string> ValidateUserAsync(User user)
        {
            if(string.IsNullOrWhiteSpace(user.Username))
            {
                return "Enter the username.";
            }
            if(string.IsNullOrWhiteSpace(user.Email))
            {
                return "Enter the email.";
            }
            if(string.IsNullOrWhiteSpace(user.Password))
            {
                return "Enter the password.";
            }
            if(await _userRepository.GetUserByEmailAsync(user.Email)!=null)
            {
                return "User with current email is exist.";
            }
            if(await _userRepository.GetUserByUsernameAsync(user.Username)!=null)
            {
                return "User with current username is exist.";
            }
            return _validationOk;
        }

        private string Hash(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash)
               .Replace("-", string.Empty)
               .ToLower();
            return encoded;
        }
    }
}
