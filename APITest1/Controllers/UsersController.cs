using APITest1.Data;
using APITest1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APITest1.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }
        
        // [HTTP Method]
        // public ActionResult<ReturnType> Name()
        // code

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(await _context.User.ToListAsync()); // status 200
        }

        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return Created("created", user);
        }

        // search?name=""&age=30
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<User>>> SearchUsers(
            [FromQuery] string? name,
            [FromQuery] string? lastName,
            [FromQuery] int? age)
        {
            var users = _context.User.AsQueryable();
            if (name != null)
            {
                users = users.Where(x => x.Name == name);
            }
            if (lastName != null)
            {
                users = users.Where(x => x.LastName == lastName);
            }
            if (age != null)
            {
                users = users.Where(x => x.Age == age);
            }

            return Ok(await users.ToListAsync());
        }
        
        //DELETE api/users/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User user)
        {
            user.Id = id;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<User>> UpdatePartialUser(int id, [FromBody] string name)
        {
            var user = await _context.User.FindAsync(id);
            user.Name = name;
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
