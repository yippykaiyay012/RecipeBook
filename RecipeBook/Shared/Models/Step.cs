using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Shared.Models
{
    public class Step
    {
        public int Id { get; set; }
        public int StepNumber { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }

      //  public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }

    }
}
