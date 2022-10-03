using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Spotify.Model;
using SpotifyStalker.Interface;
using SpotifyStalker.Service;

namespace SpotifyStalker2;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Startup));

        services.Configure<SpotifyApiSettings>(Configuration.GetSection("SpotifyApi"));

        services.AddRazorPages();
        services.AddServerSideBlazor();

        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddSingleton<IFileContentProvider, FileContentProvider>();
        services.AddSingleton<IMemoryCacheService, MemoryCacheService>();
        services.AddSingleton<IArtistProvider, ArtistProvider>();
        services.AddSingleton<IGenreProvider, GenreProvider>();
        services.AddSingleton<IRandomProvider, RandomProvider>();

        services.AddSingleton<IHttpFormPostService, HttpFormPostService>();
        services.AddSingleton<IApiRequestService, ApiRequestService>();
        services.AddSingleton<IApiRequestUrlBuilder, ApiRequestUrlBuilder>();
        services.AddSingleton<ITokenService, TokenService>();
        services.AddSingleton<IApiQueryService, ApiQueryService>();
        services.AddSingleton<IMetricProvider, MetricProvider>();

        services.AddHttpClient();
        services.AddSingleton<IAuthorizedHttpClientFactory, AuthorizedHttpClientFactory>();

        services.AddScoped<IUserQueryService, UserQueryService>();
        services.AddScoped<IUserPlaylistsQueryService, UserPlaylistsQueryService>();
        services.AddScoped<IStalkModelTransformer, StalkModelTransformer>();
        services.AddScoped<IApiBatchQueryService<ArtistModelCollection>, ApiBatchQueryService<ArtistModelCollection>>();
        services.AddScoped<IApiBatchQueryService<AudioFeaturesModelCollection>, ApiBatchQueryService<AudioFeaturesModelCollection>>();

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapBlazorHub();
            endpoints.MapFallbackToPage("/_Host");
        });

        app.UseSerilogRequestLogging();
    }
}
