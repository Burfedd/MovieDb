using Blazored.LocalStorage;
using MovieDb.Utility.Services.OmdbApi;
using MovieDb.WebApp.Services.Storage;

namespace MovieDb.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddBlazorBootstrap();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddTransient<ISavedQueriesService, SavedQueriesService>();
            builder.Services.AddSingleton<IOmdbApiService, OmdbApiService>();
            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");
            app.Run();
        }
    }
}