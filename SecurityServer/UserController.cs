using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Isopoh.Cryptography.Argon2;
using Isopoh.Cryptography.SecureArray;


namespace SecurityServer
{
    

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        securityContext db;
        private static readonly RandomNumberGenerator Rng = System.Security.Cryptography.RandomNumberGenerator.Create();
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
            
            byte[] passwordBytes = Encoding.UTF8.GetBytes(user.Password);
            byte[] salt = new byte[16];
            Rng.GetBytes(salt);
            var config = new Argon2Config
            {
                Type = Argon2Type.DataIndependentAddressing,
                Version = Argon2Version.Nineteen,
                TimeCost = 10,
                MemoryCost = 32768,
                Lanes = 5,
                Threads = Environment.ProcessorCount,
                Password = passwordBytes,
                Salt = salt,
                HashLength = 20
            };
            
            var argon2A = new Argon2(config);
            string hashString;
            using(SecureArray<byte> hashA = argon2A.Hash())
            {
                hashString = config.EncodeString(hashA.Buffer);
            }

            
            
            //user.Password = Argon2.Hash(user.Password);
            user.Password = hashString;
            

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

            //var userDb = db.User.ToListAsync().Result.Find(x => x.Name == user.Name);
            var usersDb = db.User.ToListAsync().Result.Where(x => x.Name == user.Name);

            if (usersDb.Count() == 0)
            {
                return BadRequest("Incorrect Username");
            }
            

            Console.WriteLine(user.Password);

            foreach (var userDb in usersDb)
            {
                if (Argon2.Verify(userDb.Password, user.Password))
                {
                    return Ok(userDb);
                }
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