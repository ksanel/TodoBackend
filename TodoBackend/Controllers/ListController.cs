using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoBackend.Data;
using TodoBackend.Models;
using TodoBackend.Controllers;

namespace TodoBackend.Controllers
{
    [Route("api/[controller]")]
    public class ListController : Controller
    {
        private readonly DataContext _context;
        public ListController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<List>>> GetLists()
        {
            return await _context.Lists.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List>> GetListById(int id)
        {
            return await _context.Lists.FindAsync(id);
        }

        [HttpPost]
        public async Task<ActionResult<List>> CreateList([FromBody] List list)
        {
            _context.Lists.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetListById), new { id = list.Id }, list);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List>> UpdateListById(int id, [FromBody] List list)
        {
            if (id != list.Id)
            {
                return BadRequest();
            }

            _context.Entry(list).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/todos")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetTodosByListId(int id)
        {
            var list = await _context.Lists.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }

            return await _context.Todos.Where(todo => todo.ListId == id).ToListAsync();
        }

        // Delete list and all todos in it
        [HttpDelete("{id}")]
        public async Task<ActionResult<List>> DeleteListAndTodosById(int id)
        {
            var list = await _context.Lists.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }

            var todos = await _context.Todos.Where(todo => todo.ListId == id).ToListAsync();
            foreach (var todo in todos)
            {
                _context.Todos.Remove(todo);
            }
            _context.Lists.Remove(list);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
