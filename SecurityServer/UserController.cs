using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SecurityServer
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        securityContext db;
        public UserController(securityContext context)
        {
            db = context;
            if (!db.User.Any())
            {
                db.User.Add(new User { Name = "Tom", Phone = 1234, Password = "1234", Id = 1});
                db.User.Add(new User { Name = "Alice", Phone= 312314, Password = "123123", Id = 2});
                db.SaveChanges();
            }
        }
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Get()
        {
            return await db.User.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(int id)
        {
            User user = await db.User.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return NotFound();
            return new ObjectResult(user);
        }
        
        [HttpPost]
        public async Task<ActionResult<User>> Post(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
 
            db.User.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
 
        // PUT api/users/
        [HttpPut]
        public async Task<ActionResult<User>> Put(User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (!db.User.Any(x => x.Name == user.Name))
            {
                return NotFound();
            }
 
            db.Update(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
 
        // DELETE api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> Delete(int id)
        {
            User user = db.User.FirstOrDefault(x => x.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            db.User.Remove(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
    }
}