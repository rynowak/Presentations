using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ServerApp.Data;

namespace ServerApp.Pages.NewFolder
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<TodoItem> Items { get; set; }

        public async Task OnGet()
        {
            Items = await _db.Todos.ToListAsync();
        }
    }
}
