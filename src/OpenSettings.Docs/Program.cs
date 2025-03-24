using Microsoft.AspNetCore.Builder;
using OpenSettings.Docs;
using System;
using System.IO;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string jsonExtension = ".json";
const string slashString = "/";
const char slashChar = '/';
const char vChar = 'v';

var maxAge300 = new CacheControl(300);
var maxAge600 = new CacheControl(600);

var wwwrootPath = Path.Combine(AppContext.BaseDirectory, "wwwroot");

var majorVersionToModel = Directory.GetDirectories(wwwrootPath, "v*")
    .Where(v => File.Exists(Path.Combine(v, "index.html")))
    .OrderByDescending(v => v)
    .Select(v =>
    {
        var majorVersion = Path.GetFileName(v.TrimEnd(Path.DirectorySeparatorChar));

        var files = Directory.GetFiles(v, "*", SearchOption.AllDirectories)
            .Select(file =>
            {
                var directoryName = Path.GetDirectoryName(Path.GetRelativePath(wwwrootPath, file)) ?? string.Empty;
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                var relativePath = $"/{Path.Combine(directoryName, fileNameWithoutExtension).Replace('\\', slashChar)}";
                var extension = Path.GetExtension(file)[1..];

                return new
                {
                    FullPath = file,
                    RelativePath = relativePath,
                    RelativePathWithExtension = $"{relativePath}.{extension}",
                    FileName = fileNameWithoutExtension,
                    Extension = extension
                };
            })
            .OrderBy(f => f.RelativePath)
            .ThenBy(f => GetExtensionPriority(f.Extension))
            .ToArray();

        return new
        {
            MajorVersion = majorVersion,
            IndexHtml = $"/{majorVersion}/index.html",
            PageNotFound = $"/{majorVersion}/page-not-found.html",
            Files = files
        };
    }).ToDictionary(v => v.MajorVersion);

var latestVersion = majorVersionToModel.First().Value;

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
app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 404)
    {
        context.Response.StatusCode = 200;

        if (!context.Request.Path.HasValue || context.Request.Path.Value == slashString)
        {
            context.Response.Redirect(latestVersion.IndexHtml);
        }
        else
        {
            var pathValue = context.Request.Path.Value;
            
            var requestMajorVersion = pathValue.Split(slashChar).Skip(1).First();

            var extension = Path.GetExtension(pathValue);
            var extensionWithoutDot = extension.Length > 0 ? extension[1..] : string.Empty;

            var hasExtension = !string.IsNullOrWhiteSpace(extensionWithoutDot) && !int.TryParse(extensionWithoutDot, out _);

            if (requestMajorVersion.StartsWith(vChar))
            {
                if (majorVersionToModel.TryGetValue(requestMajorVersion, out var version))
                {
                    if (hasExtension)
                    {
                        context.Response.Redirect(version.PageNotFound);
                    }
                    else
                    {
                        var matchedFile = version.Files.FirstOrDefault(r => r.RelativePathWithExtension.StartsWith(pathValue));

                        context.Response.Redirect(matchedFile?.RelativePathWithExtension ?? version.PageNotFound);
                    }
                }
                else
                {
                    context.Response.Redirect(latestVersion.PageNotFound);
                }
            }
            else
            {
                var anyMatch = false;

                foreach (var matchedFile in majorVersionToModel
                             .Select(version => version.Value.Files.FirstOrDefault(f =>
                                 hasExtension
                                     ? f.RelativePathWithExtension.EndsWith(pathValue)
                                     : f.RelativePath.EndsWith(pathValue))).Where(matchedFile => matchedFile != null))
                {
                    context.Response.Redirect(matchedFile.RelativePathWithExtension);
                    anyMatch = true;
                    break;
                }

                if (!anyMatch)
                {
                    context.Response.Redirect(latestVersion.PageNotFound);
                }
            }
        }
    }
});

app.Run();
return;

static int GetExtensionPriority(string extension)
{
    return extension.ToLowerInvariant() switch
    {
        "html" => 1,
        "json" => 2,
        "js" => 3,
        _ => 4
    };
}