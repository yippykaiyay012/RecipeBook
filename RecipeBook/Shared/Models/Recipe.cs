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

        [Required]
        [MinLength(1, ErrorMessage = "Please enter a title for your recipe")]
        public string Title { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "Please enter a description for your recipe")]
        public string Description { get; set; }

        public string IconURL { get; set; }

 
        public List<Step> Steps { get; set; } = new List<Step>();

        
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

        
        public DateTime DateCreated { get; set; }





    }
}
