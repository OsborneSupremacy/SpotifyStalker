using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using SpotifyStalker.Service;

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

        await Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHostedService<ConsoleHostedService>();

                services.AddAutoMapper(typeof(Program));

                services.Configure<SpotifyApiSettings>(configuration.GetSection("SpotifyApi"));

                services.AddHttpClient();

                services.RegisterServicesInAssembly(typeof(UserPromptService));
                services.RegisterServicesInAssembly(typeof(ApiQueryService));

                services.AddDbContext<SpotifyStalkerDbContext>();

                // TODO: Update SpotifyStalkerDbContext to accept parameters
                //services.AddDbContext<SpotifyStalkerDbContext>(options =>
                //    options.UseSqlServer(configuration.GetConnectionString("SpotifyStalker")));

            })
            .UseSerilog()
            .RunConsoleAsync();
    }

    class ConsoleHostedService : IHostedService
    {
        private readonly ILogger<ConsoleHostedService> _logger;

        private readonly IHostApplicationLifetime _appLifetime;

        private readonly UserPromptService _userPromptService;

        public ConsoleHostedService(
            ILogger<ConsoleHostedService> logger,
            IHostApplicationLifetime appLifetime,
            UserPromptService userPromptService
            )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
            _userPromptService = userPromptService ?? throw new ArgumentNullException(nameof(userPromptService));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Starting with arguments: {string.Join(" ", Environment.GetCommandLineArgs())}");

            _appLifetime.ApplicationStarted.Register(async () =>
            {
                try
                {
                    bool processing = true;
                    while (processing)
                    {
                        processing = await _userPromptService.PromptUserAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unhandled exception");
                }
                finally
                {
                    _appLifetime.StopApplication();
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
