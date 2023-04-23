using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CS54.Models;

namespace CS54.Pages_Blog
{
    public class IndexModel : PageModel
    {
        private readonly MyBlogContext _context;

        public IndexModel(MyBlogContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; } = default!;

        public const int ItemsPerPage = 5;

        [BindProperty(SupportsGet =true,Name = "p")]
        public int CurrentPage { get; set; }
        public int CountPages { get; set; }

        public async Task OnGetAsync(string searchString)
        {

            int totalArticle = await _context.Articles.CountAsync();
            CountPages= (int)Math.Ceiling((double)totalArticle/ItemsPerPage);

			if (CurrentPage < 1)
			{
				CurrentPage = 1;
			}
			if (CurrentPage > CountPages)
			{
				CurrentPage = CountPages;
			}

			var qr = await _context.Articles.OrderBy(a => a.Created)
                .Skip((CurrentPage-1)*ItemsPerPage)
                .Take(ItemsPerPage)
                .ToListAsync();
            
 
            if (!string.IsNullOrEmpty(searchString))
            {
                Article = qr.Where(a => a.Title.Contains(searchString)).ToList();
            }
            else
            {
                Article = qr.ToList();
            }
        }
    }
}
