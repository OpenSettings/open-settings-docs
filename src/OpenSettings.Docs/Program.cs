using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.DependencyInjection;
using OpenSettings.Docs;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<StaticDocsFileMap>();
builder.Services.AddSingleton<PageNotFound>();

var app = builder.Build();

const string jsonExtension = ".json";

var wwwroot = app.Environment.WebRootPath; // Path.Combine(AppContext.BaseDirectory, "wwwroot");

const string commonScriptJsFileName = "common-script.js";

var commonScript = Directory.GetFiles(wwwroot).FirstOrDefault(files => files.EndsWith(commonScriptJsFileName)) ?? throw new InvalidOperationException("File 'common-script.js' not found.");

var versions = Directory.GetDirectories(wwwroot)
    .Select(directory =>
    {
        return new { FileName = Path.GetFileName(directory), Path = directory };
    })
    .Where(directory => directory.FileName.StartsWith('v'))
    .Select(directory =>
    {
        File.Copy(commonScript, Path.Combine(directory.Path, commonScriptJsFileName), overwrite: true);

        return directory.FileName[1..];
    })
    .ToArray();


FileHelper.ReplaceTokensInFile(commonScript, new[]
{
    new KeyValuePair<string, string>("%(Versions)", JsonSerializer.Serialize(versions))
});

var maxAge300 = new CacheControl(300);
var maxAge600 = new CacheControl(600);

var rewriteOptions = new RewriteOptions();

foreach (var version in versions)
{
    rewriteOptions.AddRewrite($@"^(v{version}/docs/[^\.\s/]+)$", "$1.html", skipRemainingRules: true);
}

app.UseRewriter(rewriteOptions);
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