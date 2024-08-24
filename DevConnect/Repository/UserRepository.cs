using DevConnect.Data;
using DevConnect.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace DevConnect.Repository
{
    public class UserRepository : IUserRepository
    {
        BaseDbContext _context;
        public UserRepository(BaseDbContext context)
        {
            _context = context;
        }

        public User? AddUser(User user)
        {
            if(GetUserByEmail(user.Email)==null)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return user;
            }
            return null;
        }

        public async Task<User?> AddUserAsync(User user)
        {
            if(await GetUserByEmailAsync(user.Email)==null)
            {
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return null;
        }

        public User? GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(x => x.Email == email);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public User? GetUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public User? GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
        }

        public User? Login(string email, string password)
        {
            User? user = _context.Users.FirstOrDefault(x => x.Email == email && x.Password == password);
            return user;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
            return user;
        }

        public bool RemoveUserById(int id)
        {
            User? user = _context.Users.Find(id);
            if(user!=null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveUserByIdAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            if(user!=null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public void UpdateLastOnline(int id)
        {
            User? user = _context.Users.Find(id);
            if(user!=null)
            {
                user.LastOnline = DateOnly.FromDateTime(DateTime.Now);
                _context.Users.Update(user);
                _context.SaveChanges();
            }
        }

        public async Task UpdateLastOnlineAsync(int id)
        {
            User? user = await _context.Users.FindAsync(id);
            if(user!=null)
            {
                user.LastOnline = DateOnly.FromDateTime(DateTime.Now);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public User? UpdateUserBio(int id, string content)
        {
            User? user = _context.Users.Find(id);
            if (user != null)
            {
                user.Bio = content;
                _context.Users.Update(user);
                _context.SaveChanges();
                return user;
            }
            return user;
        }

        public async Task<User?> UpdateUserBioAsync(int id, string content)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Bio = content;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return user;
        }

        public User? UpdateUserPicture(int id, string url)
        {
            User? user = _context.Users.Find(id);
            if (user != null)
            {
                user.ProfilePicture = url;
                _context.Users.Update(user);
                _context.SaveChanges();
                return user;
            }
            return user;
        }

        public async Task<User?> UpdateUserPictureAsync(int id, string url)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.ProfilePicture = url;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return user;
        }

        public User? UpdateUserSkills(int id, string content)
        {
            User? user = _context.Users.Find(id);
            if (user != null)
            {
                user.Skills = content;
                _context.Users.Update(user);
                _context.SaveChanges();
                return user;
            }
            return user;
        }

        public async Task<User?> UpdateUserSkillsAsync(int id, string content)
        {
            User? user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Skills = content;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return user;
        }

        public async Task<User> UpdateUserAsync(int id, User user)
        {
            if(await _context.Users.FirstOrDefaultAsync(x=>x.Id==id)!=null)
            {
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            return user;
        }
    }
}
