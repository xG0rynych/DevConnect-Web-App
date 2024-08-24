using DevConnect.Models;

namespace DevConnect.Repository
{
    public interface IUserRepository
    {
        User? AddUser(User user);
        User? GetUserById(int id);
        User? GetUserByEmail(string email);
        User? GetUserByUsername(string username);
        User? UpdateUserPicture(int id, string url);
        User? UpdateUserBio(int id, string content);
        User? UpdateUserSkills(int id, string content);
        User? Login(string email, string password);
        bool RemoveUserById(int id);
        void UpdateLastOnline(int id);

        Task<User?> AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> UpdateUserPictureAsync(int id, string url);
        Task<User?> UpdateUserBioAsync(int id, string content);
        Task<User?> UpdateUserSkillsAsync(int id, string content);
        Task<User?> LoginAsync(string email, string password);
        Task<bool> RemoveUserByIdAsync(int id);
        Task UpdateLastOnlineAsync(int id);
        Task<User> UpdateUserAsync(int id, User user);
    }
}
