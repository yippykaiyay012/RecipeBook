using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Server.Data;
using RecipeBook.Shared.Models;

namespace RecipeBook.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserFavouritesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserFavouritesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserFavourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserFavourite>>> GetUserFavourites()
        {
            return await _context.UserFavourites.ToListAsync();
        }

        // GET: api/UserFavourites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserFavourite>> GetUserFavourite(int id)
        {
            var userFavourite = await _context.UserFavourites.FindAsync(id);

            if (userFavourite == null)
            {
                return NotFound();
            }

            return userFavourite;
        }

        [HttpGet("/CheckUserFavourite/")]
        public async Task<ActionResult<bool>> CheckUserFavourite([FromQuery] string userId, int recipeId) => 
            await _context.UserFavourites.AnyAsync(x => x.User.Id == userId && x.RecipeId == recipeId); 
        

        // PUT: api/UserFavourites/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserFavourite(int id, UserFavourite userFavourite)
        {
            if (id != userFavourite.Id)
            {
                return BadRequest();
            }

            _context.Entry(userFavourite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserFavouriteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/UserFavourites
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserFavourite>> PostUserFavourite(UserFavourite userFavourite)
        {
            _context.UserFavourites.Add(userFavourite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserFavourite", new { id = userFavourite.Id }, userFavourite);
        }

        // DELETE: api/UserFavourites/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserFavourite>> DeleteUserFavourite(int id)
        {
            var userFavourite = await _context.UserFavourites.FindAsync(id);
            if (userFavourite == null)
            {
                return NotFound();
            }

            _context.UserFavourites.Remove(userFavourite);
            await _context.SaveChangesAsync();

            return userFavourite;
        }

        private bool UserFavouriteExists(int id)
        {
            return _context.UserFavourites.Any(e => e.Id == id);
        }
    }
}
