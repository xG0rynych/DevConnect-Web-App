using DevConnect.Models;

namespace DevConnect.Repository
{
    public interface IMessageRepository
    {
        Task<Message?> AddAsync(Message message);
        Task<Message?> GetByIdAsync(int id);
        Task<List<Message>?> GetByUserInChatAsync(int userId, int chatId);
        Task<List<Message>?> GetByChatAsync(int chatId);
        Task<Message?> GetLastMessageByChatAsync(int chatId);
        Task<bool> Delete(int id);
        /// <summary>
        /// Delete all messages by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteByUserId(int userId);
        /// <summary>
        /// Delete all messages by chat id.
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        Task<bool> DeleteByChatId(int chatId);
    }
}
