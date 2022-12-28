using System.Threading;
using Microsoft.Extensions.Hosting;

namespace SpotifyStalker.ConsoleUi;

public class ConsoleHostedService : IHostedService
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
