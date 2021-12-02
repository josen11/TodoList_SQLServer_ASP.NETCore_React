using APIExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIExample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        //Primero hacer referencia al DBContext
        private readonly TodoContext _context;

        //Dependecy Injections
        public TodosController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/<TodosController>
        [HttpGet]
        //Async Task Action Result IEnumerable Clase
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
        {
            //Await associate to Async Task and .ToListAsync (EF methods)
            return await _context.Todos.ToListAsync();
        }

        // GET api/<TodosController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            { 
                return NotFound();
            }
            return todo;
        }

        // POST api/<TodosController>
        [HttpPost]
        //OJO podemos usar FromBody como tambien no
        //public async Task<ActionResult<Todo>> PostTodo([FromBody] Todo todo)
        public async Task<ActionResult<Todo>> PostTodo(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            //New to get the new item
            return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
        }

        // PUT api/<TodosController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>>  PutTodo(int id, Todo todo)
        {
            if (id != todo.Id)
            {
                return BadRequest();
            }
            //new
            _context.Entry(todo).State = EntityState.Modified;

            try 
            { 
                await _context.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException) 
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else {
                    throw;
                }
            }
            return todo;
        }

        private bool TodoExists(int id) {
            return _context.Todos.Any(e => e.Id == id);
        }

        // DELETE api/<TodosController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
