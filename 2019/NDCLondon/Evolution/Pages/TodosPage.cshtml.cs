using System.Collections.Generic;
using System.Threading.Tasks;
using Evolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Pages
{
    public class TodosPageModel : PageModel
    {
        private readonly ApplicationDbContext db;

        public TodosPageModel(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Todo> Todos { get; set; }

        public async Task OnGet()
        {
            Todos = await db.Todos.ToListAsync();
        }

        public async Task<ActionResult> OnPost(Todo todo)
        {
            if (ModelState.IsValid)
            {
                db.Todos.Add(todo);
                await db.SaveChangesAsync();
                return RedirectToPage();
            }

            // Validation failed
            Todos = await db.Todos.ToListAsync();
            return Page();
        }
    }
}