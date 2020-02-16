using Blazored.LocalStorage;
using Microsoft.AspNetCore.Blazor.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Client.Auth;
using System.Threading.Tasks;

namespace RecipeBook.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();
            //  builder.Services.AddScoped<AuthenticationStateProvider, DummyAuthStateProvider>();
            builder.Services.AddScoped<JWTAuthenticationProvider>();
            builder.Services.AddScoped<AuthenticationStateProvider, JWTAuthenticationProvider>(provider => 
                 provider.GetRequiredService<JWTAuthenticationProvider>());

            builder.Services.AddScoped<ILoginService, JWTAuthenticationProvider>(provider =>
                 provider.GetRequiredService<JWTAuthenticationProvider>());

            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }

        //public static IWebAssemblyHostBuilder CreateHostBuilder(string[] args) =>
        //    WebAssemblyHostBuilder.CreateDefault();
        //       // .UseBlazorStartup<Startup>();
    }
}
