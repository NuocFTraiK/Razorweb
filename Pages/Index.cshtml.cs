using CS54.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CS54.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly MyBlogContext _blogContext;

        public IndexModel(ILogger<IndexModel> logger ,MyBlogContext myBlogContext)
        {
            _logger = logger;
            _blogContext = myBlogContext;
        }

        public void OnGet()
        {
            var posts = _blogContext.Articles.OrderByDescending(a => a.Created).ToList();
            ViewData["posts"] = posts;
        }
    }
}