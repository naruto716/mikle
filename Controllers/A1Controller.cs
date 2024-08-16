using Microsoft.AspNetCore.Mvc;
using A1.Data;
using A1.Dtos;

namespace A1.Controllers
{
    [Route("webapi/[controller]")]
    [ApiController]
    public class A1Controller : ControllerBase
    {
        private readonly IA1Repo _repo;

        public A1Controller(IA1Repo repo)
        {
            _repo = repo;
        }

        [HttpGet("GetVersion")]
        public string GetVersion()
        {
            return "1.0.0 (Ngāruawāhia) by abc123";
        }

        [HttpGet("Logo")]
        public IActionResult Logo()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Logos/Logo.png");
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            return PhysicalFile(filePath, "image/png");

        }

        [HttpGet("Signs")]
        public IActionResult AllSigns()
        {
            var signs = _repo.GetAllSigns();
            return Ok(signs);
        }

        [HttpGet("Signs/{searchTerm}")]
        public IActionResult Signs(string searchTerm)
        {
            var signs = _repo.FindSigns(searchTerm);
            return Ok(signs);
        }

        [HttpGet("SignImage/{id}")]
        public IActionResult SignImage(string id)
        {
            var imagePath = _repo.GetSignImagePath(id);
            var image = System.IO.File.OpenRead(imagePath);
            var contentType = Path.GetExtension(imagePath) == ".png" ? "image/png" : "image/jpeg";
            return File(image, contentType);
        }

        [HttpGet("GetComment/{id}")]
        public IActionResult GetComment(int id)
        {
            var comment = _repo.GetCommentById(id);
            if (comment == null)
            {
                return BadRequest($"Comment {id} does not exist.");
            }
            return Ok(comment);
        }

        [HttpPost("WriteComment")]
        public IActionResult WriteComment([FromBody] CommentInput input)
        {
            if (input == null || string.IsNullOrEmpty(input.Name) || string.IsNullOrEmpty(input.UserComment))
            {
                return BadRequest("Invalid input.");
            }

            var comment = _repo.AddComment(input);
            var location = Url.Action("GetComment", new { id = comment.Id });
            Response.Headers["Location"] = location;
            return Created(location, comment);
        }

        [HttpGet("Comments/{number?}")]
        public IActionResult Comments(int? number = 5)
        {
            var comments = _repo.GetLatestComments(number.Value);
            return Ok(comments);
        }
    }
}
