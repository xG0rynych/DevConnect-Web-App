using DevConnect.Hubs;
using DevConnect.Models;
using DevConnect.Repository;
using DevConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;

namespace DevConnect.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        IHubContext<ChatHub> _hubContext;
        IUserRepository _userRepository;
        IChatRepository _chatRepository;
        IMessageRepository _messageRepository;
        public ChatController(IUserRepository userRepository, IChatRepository chatRepository, 
            IMessageRepository messageRepository, IHubContext<ChatHub> hubContext)
        {
            _userRepository = userRepository;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int id)
        {
            Chat? chat = await _chatRepository.GetByIdAsync(id);
            if (chat == null)
                return RedirectToAction("Profile", "User", new { email = User.Identity.Name });
            List<Message>? messages = await _messageRepository.GetByChatAsync(id);
            if(messages!=null)
            {
                int myUserId = (await _userRepository.GetUserByEmailAsync(User.Identity.Name)).Id;
                ChatViewModel model = new ChatViewModel();
                model.Id = id;
                model.Messages = new List<(string, bool)>();
                model.SendAt = new List<DateTime>();
                foreach (var message in messages)
                {
                    if (message.FromUserId == myUserId)
                        model.Messages.Add((message.Content, true));
                    else
                        model.Messages.Add((message.Content, false));
                    model.SendAt.Add(message.SendAt);
                }
                if (chat.FirstUserId != myUserId)
                {
                    model.FriendUsername = (await _userRepository.GetUserByIdAsync(chat.FirstUserId)).Username;
                    model.FriendId = chat.FirstUserId;
                }
                else
                {
                    model.FriendUsername = (await _userRepository.GetUserByIdAsync(chat.SecondUserId)).Username;
                    model.FriendId = chat.SecondUserId;
                }
                return View(model);
            }
            return View(null);
        }

        [HttpGet]
        public IActionResult AddChat()
        {
            return PartialView("_AddChatPartial");
        }

        [HttpPost]
        public async Task<IActionResult> AddChat(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return RedirectToAction("Profile", "User", User.Identity.Name);
            User? userSecond = await _userRepository.GetUserByUsernameAsync(username);
            if(userSecond!=null)
            {
                User userFirst = await _userRepository.GetUserByEmailAsync(User.Identity.Name);
                if((await _chatRepository.GetByUsersIdAsync(userFirst.Id, userSecond.Id)) != null)
                {
                    return RedirectToAction("Profile", "User", User.Identity.Name);
                }
                Chat chat = new Chat() { FirstUserId = userFirst.Id, SecondUserId = userSecond.Id };
                await _chatRepository.AddAsync(chat);
            }
                return RedirectToAction("Profile", "User", new { email = User.Identity.Name });
        }

        [HttpPost]
        public async Task<IActionResult> WriteMessage(Message message)
        {
            if (message==null||message.ChatId==default||string.IsNullOrWhiteSpace(message.Content)||message.ToUserId==default)
                return RedirectToAction("Index");
            message.FromUserId = (await _userRepository.GetUserByEmailAsync(User.Identity.Name)).Id;
            message.Id = default;
            message.SendAt = DateTime.Now;
            if((await _messageRepository.AddAsync(message))!=null)
            {
                string toUser = (await _userRepository.GetUserByIdAsync(message.ToUserId)).Email;
                await _hubContext.Clients.User(toUser).SendAsync("ReceiveUpdate", message.ChatId);
                return RedirectToAction("Index", new {id = message.ChatId});
            }
            return RedirectToAction("Error", "Home");
        }
    }
}
