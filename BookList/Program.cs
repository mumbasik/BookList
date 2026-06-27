namespace BookList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            builder.Services.AddControllers()
                .AddDataAnnotationsLocalization();

           
            builder.Services.AddMemoryCache(); 
            builder.Services.AddResponseCaching(); 

            var app = builder.Build();

            var supportedCultures = new[] { "en" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("en")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();

          
            app.UseResponseCaching();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}