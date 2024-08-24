using DevConnect.Data;
using DevConnect.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DevConnect.Repository
{
    public class QuestionRepository : IQuestionRepository
    {
        BaseDbContext _context;
        public QuestionRepository(BaseDbContext context)
        {
            _context = context;
        }
        public Question AddQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
            question.Author = _context.Users.Find(question.AuthorId)!;
            return question;
        }

        public async Task<Question> AddQuestionAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
            question.Author = await _context.Users.FindAsync(question.AuthorId);
            return question;
        }

        public Question? GetQuestionById(int id)
        {
            Question? question = _context.Questions.Find(id);
            if(question!=null)
            {
                question.Author = _context.Users.Find(question.AuthorId)!;
            }
            return question;
        }

        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            Question? question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                question.Author = await _context.Users.FindAsync(question.AuthorId)!;
            }
            return question;
        }

        public List<Question>? GetQuestionsByAuthorId(int id)
        {
            List<Question>? questions = _context.Questions.Where(x => x.AuthorId == id).ToList();
            if(questions!=null)
            {
                User author = _context.Users.Find(id)!;
                foreach (var question in questions)
                {
                    question.Author = author;
                }
            }
            return questions;
        }

        public async Task<List<Question>?> GetQuestionsByAuthorIdAsync(int id)
        {
            List<Question>? questions = await _context.Questions.Where(x => x.AuthorId == id).ToListAsync();
            if(questions!=null)
            {
                User author = await _context.Users.FindAsync(id);
                foreach (var question in questions)
                {
                    question.Author = author;
                }
            }
            return questions;
        }

        public List<Question>? GetQuestionsByCreatedDate(DateOnly date)
        {
            List<Question>? questions = _context.Questions.Where(x => x.CreatedAt == date).ToList();
            if (questions != null)
            {
                foreach (var question in questions)
                {
                    question.Author = _context.Users.Find(question.AuthorId)!;
                }
            }
            return questions;
        }

        public async Task<List<Question>?> GetQuestionsByCreatedDateAsync(DateOnly date)
        {
            List<Question>? questions = await _context.Questions.Where(x => x.CreatedAt == date).ToListAsync();
            if (questions != null)
            {
                foreach (var question in questions)
                {
                    question.Author = await _context.Users.FindAsync(question.AuthorId);
                }
            }
            return questions;
        }

        public List<Question>? GetQuestionsByTitle(string title)
        {
            List<Question>? questions = _context.Questions.Where(x => x.Title == title).ToList();
            if (questions != null)
            {
                foreach (var question in questions)
                {
                    question.Author = _context.Users.Find(question.AuthorId)!;
                }
            }
            return questions;
        }

        public async Task<List<Question>?> GetQuestionsByTitleAsync(string title)
        {
            List<Question>? questions = await _context.Questions.Where(x => x.Title == title).ToListAsync();
            if (questions != null)
            {
                foreach (var question in questions)
                {
                    question.Author = await _context.Users.FindAsync(question.AuthorId);
                }
            }
            return questions;
        }

        public bool RemoveQuestionById(int id)
        {
            Question? question = _context.Questions.Find(id);
            if(question!=null)
            {
                _context.Comments.RemoveRange(_context.Comments.Where(x => x.QuestionId == id).ToList());
                _context.Questions.Remove(question);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveQuestionByIdAsync(int id)
        {
            Question? question = await _context.Questions.FindAsync(id);
            if(question!=null)
            {
                _context.Comments.RemoveRange(await _context.Comments.Where(x => x.QuestionId == id).ToListAsync());
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public bool RemoveQuestionsByAuthorId(int id)
        {
            List<Question>? questions = _context.Questions.Where(x => x.AuthorId == id).ToList();
            if(questions!=null)
            {
                foreach (var question in questions)
                {
                    List<Comment>? comments = _context.Comments.Where(x => x.QuestionId == question.Id).ToList();
                    if(comments!=null)
                    {
                        _context.Comments.RemoveRange(comments);
                    }
                }
                _context.Questions.RemoveRange(questions);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveQuestionsByAuthorIdAsync(int id)
        {
            List<Question>? questions = await _context.Questions.Where(x => x.AuthorId == id).ToListAsync();
            if(questions!=null)
            {
                foreach (var question in questions)
                {
                    List<Comment>? comments = await _context.Comments.Where(x => x.QuestionId == question.Id).ToListAsync();
                    if (comments != null)
                    {
                        _context.Comments.RemoveRange(comments);
                    }
                }
                _context.Questions.RemoveRange(questions);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public List<Question>? GetNewestQuestions(int quantity)
        {
            List<Question>? questions = _context.Questions.OrderByDescending(x => x.Id).Take(quantity).ToList();
            if(questions!=null)
            {
                foreach (var question in questions)
                {
                    question.Author = _context.Users.Find(question.AuthorId)!;
                }
            }
            return questions;
        }

        public async Task<List<Question>?> GetNewestQuestionsAsync(int quantity)
        {
            List<Question>? questions = await _context.Questions.OrderByDescending(x => x.Id).Take(quantity).ToListAsync();
            if (questions != null)
            {
                foreach (var question in questions)
                {
                    question.Author = await _context.Users.FindAsync(question.AuthorId);
                }
            }
            return questions;
        }
    }
}
