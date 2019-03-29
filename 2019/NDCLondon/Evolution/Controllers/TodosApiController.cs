using System.Collections.Generic;
using System.Threading.Tasks;
using Evolution.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Evolution.Controllers
{
    [ApiController]
    [Route("api/todos")]
    public class TodosApiController : Controller
    {
        private readonly ApplicationDbContext db;

        public TodosApiController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Todo>>> GetAll()
        {
            return await db.Todos.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> Create(Todo todo)
        {
            db.Todos.Add(todo);
            await db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAll), todo);
        }
    }
}
