﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeBook.Server.Data;
using RecipeBook.Shared.Models;


namespace RecipeBook.Server.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecipesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: api/Recipes
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        //{
        //    var query = this.Request.Query;
        //    return await _context.Recipes.ToListAsync();
        //}


        [HttpGet("/api/recipes/search")]
        public async Task<ActionResult<PagedResult<Recipe>>> GetRecipes([FromQuery]string searchTerm, int pageNumber = 1, int pageSize = 10)
        {

            if (searchTerm == null)
            {
                searchTerm = string.Empty;
            }
            var totalRecords =  await _context.Recipes.Where(x => x.Title.Contains(searchTerm) || x.Description.Contains(searchTerm)).CountAsync();
            var maxPages = totalRecords / pageSize;

            var results = await _context.Recipes
                .Where(x => x.Title.Contains(searchTerm) || x.Description.Contains(searchTerm))
                .Skip((pageNumber -1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Recipe>
            {
                Page = pageNumber,
                MaxPages = maxPages,
                PageSize = pageSize,
                Items = results
            };

        }

        // GET: api/Recipes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
             return await _context.Recipes.ToListAsync();
        }

        //[HttpGet("/api/recipes/search")]
        //public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes([FromQuery]string searchTerm)
        //{
        //    if (searchTerm == null)
        //        return await _context.Recipes.ToListAsync();

        //    return await _context.Recipes
        //        .Where(x => x.Title.Contains(searchTerm)
        //            || x.Description.Contains(searchTerm)).ToListAsync();
        //}




        // GET: api/Recipes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes
                .Include(x => x.Ingredients)
                .Include(x => x.Steps)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }


            foreach(var ingr in recipe.Ingredients)
            {
                Console.WriteLine(ingr.Name);
            }

            return recipe;
        }

        // PUT: api/Recipes/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecipe(int id, Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return BadRequest();
            }

            _context.Entry(recipe).State = EntityState.Modified;

           

            foreach(var step in recipe.Steps)
            {
                if(step.Id == 0)
                {
                    _context.Steps.Add(step);
                }
                else
                {
                    _context.Entry(step).State = EntityState.Modified;
                }
            }

            foreach (var ingr in recipe.Ingredients)
            {
                if (ingr.Id == 0)
                {
                    _context.Ingredients.Add(ingr);
                }
                else
                {
                    _context.Entry(ingr).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeExists(id))
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

        // POST: api/Recipes
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Recipe>> PostRecipe(Recipe recipe)
        {
            recipe.DateCreated = DateTime.Now;
            _ = recipe.IconURL ?? (recipe.IconURL = "http://icons.iconarchive.com/icons/webalys/kameleon.pics/256/Food-Dome-icon.png");

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        }

        // DELETE: api/Recipes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Recipe>> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return recipe;
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
