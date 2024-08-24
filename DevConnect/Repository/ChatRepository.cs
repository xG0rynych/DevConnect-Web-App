using DevConnect.Data;
using DevConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Repository
{
    public class ChatRepository : IChatRepository
    {
        BaseDbContext _context;
        public ChatRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<Chat?> AddAsync(Chat chat)
        {
            if (chat != null && await _context.Chats.FirstOrDefaultAsync(x=>x.Id==chat.Id) == null)
            {
                await _context.Chats.AddAsync(chat);
                await _context.SaveChangesAsync();
                return chat;
            }
            return null;
        }

        public async Task<bool> Delete(int id)
        {
            Chat? chat = await _context.Chats.FirstOrDefaultAsync(x => x.Id == id);
            if (chat != null)
            {
                List<Message>? messages = await _context.Messages.Where(x => x.ChatId == id).ToListAsync();
                _context.Chats.Remove(chat);
                _context.Messages.RemoveRange(messages);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteByUserId(int userId)
        {
            List<Chat>? chats = await _context.Chats.Where(x => x.FirstUserId == userId || x.SecondUserId == userId).ToListAsync();
            if (chats != null)
            {
                foreach (var chat in chats)
                {
                    _context.Messages.RemoveRange(await _context.Messages.Where(x => x.ChatId == chat.Id).ToListAsync());
                }
                _context.Chats.RemoveRange(chats);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteByUsersId(int firstUserID, int secondUserId)
        {
            Chat? chat = await _context.Chats.FirstOrDefaultAsync(x => x.FirstUserId == firstUserID && x.SecondUserId == secondUserId);
            if (chat != null)
            {
                List<Message>? messages = await _context.Messages.Where(x => x.ChatId == chat.Id).ToListAsync();
                _context.Chats.Remove(chat);
                _context.Messages.RemoveRange(messages);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Chat?> GetByIdAsync(int id)
        {
            return await _context.Chats.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Chat>?> GetByUserAsync(int userId)
        {
            return await _context.Chats.Where(x => x.FirstUserId == userId || x.SecondUserId == userId).ToListAsync();
        }

        public async Task<Chat?> GetByUsersIdAsync(int firstUserID, int secondUserId)
        {
            return await _context.Chats.FirstOrDefaultAsync(x => (x.FirstUserId == firstUserID && x.SecondUserId == secondUserId) || 
            (x.FirstUserId==secondUserId && x.SecondUserId==firstUserID));
        }
    }
}
