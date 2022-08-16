using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Core.Dtos.BlogsDtos
{
    public class BlogCreateEditDto
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Title { get; set; }

        [Required, MaxLength(5000)]
        public string BlogContent { get; set; }
        public IFormFile Picture { get; set; }
    }
}