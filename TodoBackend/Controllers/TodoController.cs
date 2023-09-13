using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBackend.Data;
using TodoBackend.Models;

namespace TodoBackend.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly DataContext _context;

        public TodoController(DataContext context)
        {
            _context = context;
        }

        // GET api/todo 
        // params : limit, skip
        [HttpGet]
        public async Task<ActionResult<List<Todo>>> GetTodos(int limit = 10, int skip = 0)
        {
            // Get all todos count
            var count = await _context.Todos.CountAsync();
            // Return a list of todos from the database as a json response with count
            return Ok(new { total = count, todos = await _context.Todos.Skip(skip).Take(limit).ToListAsync() });

        }
        

        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetTodoById(int id)
        {
            // Return a single todo from the database as a json response
            return await _context.Todos.FindAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<Todo>> CreateTodo([FromBody] Todo todo)
        {
            // Create a new todo in the database and return it as a json response
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> UpdateTodoById(int id, [FromBody] Todo todo)
        {
            // Update a todo in the database and return it as a json response
            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Todo>>> Delete(int id)
        {
            // Delet item
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
