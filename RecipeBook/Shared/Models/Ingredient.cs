using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Shared.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Measurement { get; set; }
        public string Name { get; set; }
      //  public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }

    }
}
