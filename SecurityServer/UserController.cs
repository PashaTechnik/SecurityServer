using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Isopoh.Cryptography.Argon2;


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
            
            user.Password = Argon2.Hash(user.Password);

            db.User.Add(user);
            await db.SaveChangesAsync();
            return Ok(user);
        }
        
        [HttpPost]
        [Route("Verify")]
        public async Task<ActionResult<User>> Verify(User user)
        {
            Console.WriteLine(user.Password);
            if (user.Password == "")
            {
                return BadRequest();
            }

            var userDb = db.User.ToListAsync().Result.Find(x => x.Name == user.Name);

            if (userDb == null)
            {
                return BadRequest("Incorrect Username");
            }
            
            if (Argon2.Verify(userDb.Password, user.Password))
            {
                return Ok(userDb);
            }
            
            return BadRequest("Incorrect Password");
            // user.Password = Argon2.Hash(user.Password);
            //
            // db.User.Add(user);
            // await db.SaveChangesAsync();
            // return Ok(user);
        }
        
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