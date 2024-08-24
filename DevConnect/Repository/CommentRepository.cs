using DevConnect.Data;
using DevConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace DevConnect.Repository
{
    public class CommentRepository : ICommentRepository
    {
        BaseDbContext _context;
        public CommentRepository(BaseDbContext context)
        {
            _context = context;
        }

        public Comment AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            comment.Author = _context.Users.Find(comment.AuthorId)!;
            _context.SaveChanges();
            return comment;
        }

        public async Task<Comment> AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            comment.Author = await _context.Users.FindAsync(comment.AuthorId);
            await _context.SaveChangesAsync();
            return comment;
        }

        public Comment? GetCommentById(int id)
        {
            Comment? comment = _context.Comments.Find(id);
            if(comment!=null)
            {
                comment.Author = _context.Users.Find(comment.AuthorId)!;
            }
            return comment;
        }

        public async Task<Comment?> GetCommentByIdAsync(int id)
        {
            Comment? comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                comment.Author = await _context.Users.FindAsync(comment.AuthorId);
            }
            return comment;
        }

        public List<Comment>? GetCommentsByArticleId(int id)
        {
            List<Comment>? comments = _context.Comments.Where(x => x.ArticleId == id).ToList();
            if(comments!=null)
            {
                foreach (var comment in comments)
                {
                    comment.Author = _context.Users.Find(comment.AuthorId)!;
                }
            }
            return comments;
        }

        public async Task<List<Comment>?> GetCommentsByArticleIdAsync(int id)
        {
            List<Comment>? comments = await _context.Comments.Where(x => x.ArticleId == id).ToListAsync();
            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    comment.Author = await _context.Users.FindAsync(comment.AuthorId);
                }
            }
            return comments;
        }

        public List<Comment>? GetCommentsByAuthorId(int id)
        {
            List<Comment>? comments = _context.Comments.Where(x => x.AuthorId == id).ToList();
            if(comments!=null)
            {
                User author = _context.Users.Find(id)!;
                foreach (var comment in comments)
                {
                    comment.Author = author;
                }
            }
            return comments;
        }

        public async Task<List<Comment>?> GetCommentsByAuthorIdAsync(int id)
        {
            List<Comment>? comments = await _context.Comments.Where(x => x.AuthorId == id).ToListAsync();
            if (comments != null)
            {
                User author = await _context.Users.FindAsync(id);
                foreach (var comment in comments)
                {
                    comment.Author = author!;
                }
            }
            return comments;
        }

        public List<Comment>? GetCommentsByCreatedDate(DateOnly date)
        {
            List<Comment>? comments = _context.Comments.Where(x => x.CreatedAt == date).ToList();
            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    comment.Author = _context.Users.Find(comment.AuthorId)!;
                }
            }
            return comments;
        }

        public async Task<List<Comment>?> GetCommentsByCreatedDateAsync(DateOnly date)
        {
            List<Comment>? comments = await _context.Comments.Where(x => x.CreatedAt == date).ToListAsync();
            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    comment.Author = await _context.Users.FindAsync(comment.AuthorId);
                }
            }
            return comments;
        }

        public List<Comment>? GetCommentsByQuestionId(int id)
        {
            List<Comment>? comments = _context.Comments.Where(x => x.QuestionId == id).ToList();
            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    comment.Author = _context.Users.Find(comment.AuthorId)!;
                }
            }
            return comments;
        }

        public async Task<List<Comment>?> GetCommentsByQuestionIdAsync(int id)
        {
            List<Comment>? comments = await _context.Comments.Where(x => x.QuestionId == id).ToListAsync();
            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    comment.Author = await _context.Users.FindAsync(comment.AuthorId);
                }
            }
            return comments;
        }

        public bool RemoveCommentById(int id)
        {
            Comment? comment = _context.Comments.Find(id);
            if(comment!=null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveCommentByIdAsync(int id)
        {
            Comment? comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool RemoveCommentsByAuthorId(int id)
        {
            List<Comment>? comments = _context.Comments.Where(x => x.AuthorId == id).ToList();
            if(comments!=null)
            {
                _context.Comments.RemoveRange(comments);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveCommentsByAuthorIdAsync(int id)
        {
            List<Comment>? comments = await _context.Comments.Where(x => x.AuthorId == id).ToListAsync();
            if (comments != null)
            {
                _context.Comments.RemoveRange(comments);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
