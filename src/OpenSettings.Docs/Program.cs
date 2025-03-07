using Microsoft.AspNetCore.Builder;
using System;
using System.IO;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var wwwrootPath = Path.Combine(Environment.CurrentDirectory, "wwwroot");

var majorVersionToModel = Directory.GetDirectories(wwwrootPath, "v*")
    .Where(v => File.Exists(Path.Combine(v, "index.html")))
    .OrderByDescending(v => v)
    .Select(v =>
    {
        var majorVersion = Path.GetFileName(v.TrimEnd(Path.DirectorySeparatorChar));

        return new
        {
            MajorVersion = majorVersion,
            IndexHtml = $"/{majorVersion}/index.html",
            PageNotFound = $"/{majorVersion}/page-not-found.html"
        };
    }).ToDictionary(v => v.MajorVersion);

var latestVersion = majorVersionToModel.First().Value;

app.UseDefaultFiles();
app.UseStaticFiles();
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Response.StatusCode = 200;

        if (!context.Request.Path.HasValue || context.Request.Path.Value == "/")
        {
            context.Response.Redirect(latestVersion.IndexHtml);
        }
        else
        {
            var requestMajorVersion = context.Request.Path.Value.Split('/').Skip(1).First();

            context.Response.Redirect(majorVersionToModel.TryGetValue(requestMajorVersion, out var version)
                ? version.PageNotFound
                : latestVersion.PageNotFound);
        }
    }
});


app.Run();