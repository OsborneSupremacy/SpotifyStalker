using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using SpotifyStalker.Service;
using OsborneSupremacy.Extensions.AspNet;

namespace SpotifyStalker.ConsoleUi;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");

        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
            .Equals("Development", StringComparison.OrdinalIgnoreCase))
            builder.AddUserSecrets<Program>();

        var configuration = builder.Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        var spotifySettings = configuration
            .GetAndValidateTypedSection("SpotifyApi", new SpotifyApiSettingsValidator());

        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsoleHostedService>();

                services.AddAutoMapper(typeof(Program));

                services.AddSingleton(spotifySettings);

                services.AddHttpClient();

                services.RegisterServicesInAssembly(typeof(UserPromptService));
                services.RegisterServicesInAssembly(typeof(ApiQueryService));

                services.AddDbContext<SpotifyStalkerDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SpotifyStalker"))
                );

            })
            .UseSerilog()
            .RunConsoleAsync();
    }
}
