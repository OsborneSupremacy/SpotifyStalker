using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OsborneSupremacy.Extensions.AspNet;
using OsborneSupremacy.Extensions.Net.DependencyInjection;
using Serilog;
using Spotify.Model;
using SpotifyStalker.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddSingleton(
    builder
        .GetAndValidateTypedSection("SpotifyApi", new SpotifyApiSettingsValidator())
);

builder.Services.RegisterServicesInAssembly(typeof(ApiQueryService));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
