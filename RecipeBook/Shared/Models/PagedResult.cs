using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Shared.Models
{
    public class PagedResult<T>
    {
        public int MaxPages { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
        public List<T> Items { get; set; }

    }
}
