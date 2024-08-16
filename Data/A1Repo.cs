using A1.Dtos;
using A1.Models;

namespace A1.Data
{
    public class A1Repo : IA1Repo
    {
        private readonly A1DbContext _context;

        public A1Repo(A1DbContext context)
        {
            _context = context;
        }

        public List<Sign> GetAllSigns()
        {
            return _context.Signs.ToList();
        }

        public IEnumerable<Sign> FindSigns(string searchTerm)
        {
            return _context.Signs
                .Where(s => s.Description.Contains(searchTerm))
                .ToList();
        }

        public string GetSignImagePath(string id)
        {
            var path = Path.Combine("SignsImages", id + ".png");
            return File.Exists(path) ? path : Path.Combine("SignsImages", "default.png");
        }

        public Comment GetCommentById(int id)
        {
            return _context.Comments.FirstOrDefault(c => c.Id == id);
        }

        public Comment AddComment(CommentInput input)
        {
            var comment = new Comment
            {
                Name = input.Name,
                UserComment = input.UserComment,
                Time = System.DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ"),
                Ip = "127.0.0.1" // Placeholder for actual IP retrieval
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return comment;
        }

        public IEnumerable<Comment> GetLatestComments(int number)
        {
            return _context.Comments
                .OrderByDescending(c => c.Id)
                .Take(number)
                .ToList();
        }
    }
}