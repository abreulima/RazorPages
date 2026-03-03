namespace RazorPages
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<DAL.Repositories.UserRepository>();
            builder.Services.AddScoped<DAL.Services.UserService>();

            builder.Services.AddScoped<DAL.Repositories.IngredientRepository>();
            builder.Services.AddScoped<DAL.Services.IngredientService>();

            builder.Services.AddScoped<DAL.Repositories.CategoryRepository>();
            builder.Services.AddScoped<DAL.Services.CategoryService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            // Permite acesso ao arquivos estáicos na pasta wwwroot
            app.UseStaticFiles();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
