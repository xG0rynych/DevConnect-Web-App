using DevConnect.Models;

namespace DevConnect.Repository
{
    public interface IChatRepository
    {
        Task<Chat?> AddAsync(Chat chat);
        Task<Chat?> GetByIdAsync(int id);
        Task<Chat?> GetByUsersIdAsync(int firstUserID, int secondUserId);
        Task<List<Chat>?> GetByUserAsync(int userId);
        Task<bool> Delete(int id);
        /// <summary>
        /// Delete chat by users id.
        /// </summary>
        /// <param name="firstUserID"></param>
        /// <param name="secondUserId"></param>
        /// <returns></returns>
        Task<bool> DeleteByUsersId(int firstUserID, int secondUserId);
        /// <summary>
        /// Deleteall chats by user id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<bool> DeleteByUserId(int userId);
    }
}
