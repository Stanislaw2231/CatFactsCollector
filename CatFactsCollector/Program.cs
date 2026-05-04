using CatFactsCollector.Contracts;
using CatFactsCollector.Services;

namespace CatFactsCollector;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddSingleton<IFileService, FileService>();

        var config = builder.Configuration;
        var filePath = config["FilePath"];
        var baseUrl = config["BaseUrl"];

        if (string.IsNullOrWhiteSpace(filePath) || string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException("Settings FilePath and BaseUrl are required.");
        }

        builder.Services.AddHttpClient<ICatFactService, CatFactService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(10);
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapControllers();
        app.UseHttpsRedirection();
        
        app.UseAuthorization();
        

        app.Run();
    }
}
