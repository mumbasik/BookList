namespace BookList
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddLocalization(options => options.ResourcesPath = "");

            
            builder.Services.AddControllers().AddDataAnnotationsLocalization();

            var app = builder.Build();

         
            var supportedCultures = new[] { "en" };
            var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture("en").AddSupportedCultures(supportedCultures).AddSupportedUICultures(supportedCultures);

            app.UseRequestLocalization(localizationOptions);

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}