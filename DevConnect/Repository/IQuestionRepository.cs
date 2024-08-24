using DevConnect.Models;

namespace DevConnect.Repository
{
    public interface IQuestionRepository
    {
        Question AddQuestion(Question question);
        Question? GetQuestionById(int id);
        List<Question>? GetQuestionsByAuthorId(int id);
        List<Question>? GetQuestionsByTitle(string title);
        List<Question>? GetQuestionsByCreatedDate(DateOnly date);
        List<Question>? GetNewestQuestions(int quantity);
        bool RemoveQuestionById(int id);
        bool RemoveQuestionsByAuthorId(int id);

        Task<Question> AddQuestionAsync(Question question);
        Task<Question?> GetQuestionByIdAsync(int id);
        Task<List<Question>?> GetQuestionsByAuthorIdAsync(int id);
        Task<List<Question>?> GetQuestionsByTitleAsync(string title);
        Task<List<Question>?> GetQuestionsByCreatedDateAsync(DateOnly date);
        Task<List<Question>?> GetNewestQuestionsAsync(int quantity);
        Task<bool> RemoveQuestionByIdAsync(int id);
        Task<bool> RemoveQuestionsByAuthorIdAsync(int id);
    }
}
