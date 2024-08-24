using Microsoft.EntityFrameworkCore;
using DevConnect.Models;
using DevConnect.Data;

namespace DevConnect.Repository
{
    public class MessageRepository : IMessageRepository
    {
        BaseDbContext _context;
        public MessageRepository(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<Message?> AddAsync(Message message)
        {
            if (message != null && await _context.Messages.FirstOrDefaultAsync(x=>x.Id==message.Id) == null)
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();
                return message;
            }
            return null;
        }

        public async Task<bool> Delete(int id)
        {
            Message? message = await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
            if (message != null)
            {
                _context.Messages.Remove(message);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteByChatId(int chatId)
        {
            List<Message>? messages = await _context.Messages.Where(x => x.ChatId == chatId).ToListAsync();
            if (messages != null)
            {
                _context.Messages.RemoveRange(messages);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteByUserId(int userId)
        {
            List<Message>? messages = await _context.Messages.Where(x => x.FromUserId == userId).ToListAsync();
            if (messages != null)
            {
                _context.Messages.RemoveRange(messages);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<Message>?> GetByChatAsync(int chatId)
        {
            return await _context.Messages.Where(x => x.ChatId == chatId).ToListAsync();
        }

        public async Task<Message?> GetByIdAsync(int id)
        {
            return await _context.Messages.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Message>?> GetByUserInChatAsync(int userId, int chatId)
        {
            return await _context.Messages.Where(x => x.Id == chatId && x.FromUserId == userId).ToListAsync();
        }

        public async Task<Message?> GetLastMessageByChatAsync(int chatId)
        {
            Message? message = await _context.Messages.Where(x => x.ChatId == chatId).OrderByDescending(x => x.SendAt)
                .FirstOrDefaultAsync();
            return message;
        }
    }
}
