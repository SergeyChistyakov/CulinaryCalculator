using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace CulinaryCloud
{
    public class FileManagerController : Controller
    {
        private const string m_UploadDirectory = "c:/CulinaryFiles";

        [HttpPost("/file")]
        [Authorize]
        public IActionResult PostFile([FromForm(Name = "file")] IFormFile file)
        {
            if (!Directory.Exists(m_UploadDirectory))
                Directory.CreateDirectory(m_UploadDirectory);

            var fileName = GetFileName(User.Identity.Name);
            var filePath = Path.Combine(m_UploadDirectory, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return new OkResult();
        }

        [HttpGet("/file")]
        [Authorize]
        public IActionResult GetFile()
        {
            var fileName = GetFileName(User.Identity.Name);
            Stream stream = System.IO.File.OpenRead(Path.Combine(m_UploadDirectory, fileName));

            if (stream == null) return NotFound();

            return File(stream, "application/octet-stream");
        }

        private string GetFileName(string login)
        {
            return login.Replace("@", "_") + ".db";
        }
    }
}
