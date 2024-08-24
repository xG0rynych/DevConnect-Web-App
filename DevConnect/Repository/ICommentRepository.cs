using DevConnect.Models;

namespace DevConnect.Repository
{
    public interface ICommentRepository
    {
        Comment AddComment(Comment comment);
        Comment? GetCommentById(int id);
        List<Comment>? GetCommentsByAuthorId(int id);
        List<Comment>? GetCommentsByArticleId(int id);
        List<Comment>? GetCommentsByQuestionId(int id);
        List<Comment>? GetCommentsByCreatedDate(DateOnly date);
        bool RemoveCommentById(int id);
        bool RemoveCommentsByAuthorId(int id);

        Task<Comment> AddCommentAsync(Comment comment);
        Task<Comment?> GetCommentByIdAsync(int id);
        Task<List<Comment>?> GetCommentsByAuthorIdAsync(int id);
        Task<List<Comment>?> GetCommentsByArticleIdAsync(int id);
        Task<List<Comment>?> GetCommentsByQuestionIdAsync(int id);
        Task<List<Comment>?> GetCommentsByCreatedDateAsync(DateOnly date);
        Task<bool> RemoveCommentByIdAsync(int id);
        Task<bool> RemoveCommentsByAuthorIdAsync(int id);
    }
}
