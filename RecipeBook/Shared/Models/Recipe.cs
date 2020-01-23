using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RecipeBook.Shared.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string IconURL { get; set; }
        public List<Step> Steps { get; set; } = new List<Step>();
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        
        public DateTime DateCreated { get; set; }





    }
}
