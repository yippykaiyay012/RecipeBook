using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Shared.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconURL { get; set; }
        public List<Step> Steps { get; set; }
        public List<Ingredient> Ingredients { get; set; }

    }
}
