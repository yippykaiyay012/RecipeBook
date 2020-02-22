using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Shared.Models
{
    public class UserFavourite
    {
        public int Id { get; set; }
        public Recipe Recipe { get; set; }
        public int RecipeId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
