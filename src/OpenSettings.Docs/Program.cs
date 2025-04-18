using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OpenSettings.Docs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<StaticDocsFileMap>();
builder.Services.AddSingleton<PageNotFound>();

var app = builder.Build();

const string jsonExtension = ".json";

var maxAge300 = new CacheControl(300);
var maxAge600 = new CacheControl(600);

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = context =>
    {
        var cacheControl = context.File.Name.EndsWith(jsonExtension)
            ? maxAge300
            : maxAge600;

        context.Context.Response.Headers.CacheControl = cacheControl.ToString();
        context.Context.Response.Headers.Expires = cacheControl.GetHttpExpiresHeader();
    }
});
app.UseMiddleware<FallbackPageMiddleware>();
app.Run();