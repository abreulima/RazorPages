using DAL.Repositories;

namespace RazorPages
{

    // Session
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-10.0

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDistributedMemoryCache();


            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(21);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            // Adiciona 
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<DAL.Repositories.UserRepository>();
            builder.Services.AddScoped<DAL.Services.UserService>();

            builder.Services.AddScoped<DAL.Repositories.IngredientRepository>();
            builder.Services.AddScoped<DAL.Services.IngredientService>();

            builder.Services.AddScoped<DAL.Repositories.CategoryRepository>();
            builder.Services.AddScoped<DAL.Services.CategoryService>();

            builder.Services.AddScoped<DAL.Repositories.DifficultRepository>();
            builder.Services.AddScoped<DAL.Services.DifficultService>();

            builder.Services.AddScoped<DAL.Repositories.RecipeRepository>();
            builder.Services.AddScoped<DAL.Services.RecipeService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // Permite acesso ao arquivos estáicos na pasta wwwroot
            app.UseStaticFiles();

            // Permite o uso de sessões, verifique método AddSession acima
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();


            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
