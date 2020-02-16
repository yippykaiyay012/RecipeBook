using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeBook.Client.Auth
{
    public interface ILoginService
    {
        Task SignIn(string token);

        Task SignOut(); 
    }
}
