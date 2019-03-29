using System.Threading.Tasks;
using Evolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Controllers
{
    public class TodosController : Controller
    {
        private readonly ApplicationDbContext db;

        public TodosController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var todos = await db.Todos.ToListAsync();
            return View(new TodosViewModel() { Todos = todos, });
        }

        public async Task<IActionResult> Index(Todo todo)
        {
            if (ModelState.IsValid)
            {
                db.Todos.Add(todo);
                await db.SaveChangesAsync();
                return RedirectToAction();
            }

            // Validation failed
            var todos = await db.Todos.ToListAsync();
            return View(todos);
        }
    }
}
