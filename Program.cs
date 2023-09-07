namespace MinesweeperBackend {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors();
            builder.Services.AddControllers()
                .AddNewtonsoftJson();

            var app = builder.Build();

            app.UseCors(builder => builder
             .AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());

            app.MapControllers();
            app.UseHttpsRedirection();
            app.Run();
        }
    }
}