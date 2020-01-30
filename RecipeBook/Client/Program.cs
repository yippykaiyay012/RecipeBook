using Microsoft.AspNetCore.Blazor.Hosting;
using System.Threading.Tasks;

namespace RecipeBook.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }

        //public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
        //    WebAssemblyHostBuilder.CreateDefault();
        //       // .UseBlazorStartup<Startup>();
    }
}
