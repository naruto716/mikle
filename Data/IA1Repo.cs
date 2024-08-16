using A1.Models;
using A1.Dtos;

namespace A1.Data
{
    public interface IA1Repo
    {
        List<Sign> GetAllSigns();
        IEnumerable<Sign> FindSigns(string searchTerm);
        string GetSignImagePath(string id);
        Comment GetCommentById(int id);
        Comment AddComment(CommentInput input);
        IEnumerable<Comment> GetLatestComments(int number);
    }
}