using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OpenSettings.Docs
{
    public class StaticDocsFileMap
    {
        private const char SlashChar = '/';
        private const char BackslashChar = '\\';
        private const string HtmlExtension = "html";
        private const string IndexHtml = "index.html";
        private const string VersionSearchPattern = "v*";

        public StaticDocsFileMap(IWebHostEnvironment webHostEnvironment)
        {
            var flattenedRelativePathsWithExtension = new List<string>();
            var flattenedRelativeHtmlPaths = new List<string>();

            MajorVersionToDocSet = Directory.GetDirectories(webHostEnvironment.WebRootPath, VersionSearchPattern)
                .Where(v => File.Exists(Path.Combine(v, IndexHtml)))
                .OrderByDescending(v => v)
                .Select(v =>
                {
                    var majorVersion = Path.GetFileName(v.TrimEnd(Path.DirectorySeparatorChar))?.ToLowerInvariant();

                    var relativePathsWithExtension = new List<string>();
                    var relativeHtmlPaths = new List<string>();

                    var files = Directory.GetFiles(v, "*", SearchOption.AllDirectories)
                        .Select(file =>
                        {
                            var directoryName = Path.GetDirectoryName(Path.GetRelativePath(webHostEnvironment.WebRootPath, file)) ?? string.Empty;
                            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file);
                            var relativePath = $"/{Path.Combine(directoryName, fileNameWithoutExtension).Replace(BackslashChar, SlashChar)}";
                            var extension = Path.GetExtension(file)[1..];
                            bool hasExtension, isHtmlFile;
                            string relativePathWithExtension;

                            if (!string.IsNullOrWhiteSpace(extension))
                            {
                                hasExtension = true;
                                relativePathWithExtension = $"{relativePath}.{extension}";

                                isHtmlFile = extension == HtmlExtension;
                            }
                            else
                            {
                                hasExtension = isHtmlFile = false;
                                relativePathWithExtension = relativePath;
                            }

                            return new DocFileEntry
                            {
                                FullPath = file,
                                RelativePath = relativePath,
                                RelativePathWithExtension = relativePathWithExtension,
                                FileName = fileNameWithoutExtension,
                                Extension = extension,
                                HasExtension = hasExtension,
                                IsHtmlFile = isHtmlFile
                            };
                        })
                        .OrderBy(f => f.RelativePath)
                        .ThenBy(f => GetExtensionPriority(f.Extension))
                        .Select(f =>
                        {
                            relativePathsWithExtension.Add(f.RelativePathWithExtension);
                            flattenedRelativePathsWithExtension.Add(f.RelativePathWithExtension);

                            if (!f.IsHtmlFile)
                            {
                                return f;
                            }

                            relativeHtmlPaths.Add(f.RelativePathWithExtension);
                            flattenedRelativeHtmlPaths.Add(f.RelativePathWithExtension);

                            return f;
                        })
                        .ToArray();

                    var indexHtml = $"/{majorVersion}/index.html";
                    var pageNotFound = $"/{majorVersion}/page-not-found.html";

                    return new VersionedDocSet
                    {
                        MajorVersion = majorVersion,
                        IndexHtml = indexHtml,
                        IndexHtmlFilePath = Path.Combine(webHostEnvironment.WebRootPath, indexHtml[1..]),
                        PageNotFound = pageNotFound,
                        PageNotFoundFilePath = Path.Combine(webHostEnvironment.WebRootPath, pageNotFound[1..]),
                        Files = files,
                        RelativePathsWithExtension = relativePathsWithExtension.ToArray(),
                        RelativeHtmlPaths = relativeHtmlPaths.ToArray()
                    };
                }).ToDictionary(v => v.MajorVersion);

            LatestVersion = MajorVersionToDocSet.FirstOrDefault().Value;

            RelativePathsWithExtension = flattenedRelativePathsWithExtension.ToArray();
            RelativeHtmlPaths = flattenedRelativeHtmlPaths.ToArray();
        }

        public Dictionary<string, VersionedDocSet> MajorVersionToDocSet { get; }

        public VersionedDocSet LatestVersion { get; }

        public string[] RelativePathsWithExtension { get; }

        public string[] RelativeHtmlPaths { get; }

        public static int GetExtensionPriority(string extension)
        {
            return extension.ToLowerInvariant() switch
            {
                "html" => 1,
                "json" => 2,
                "js" => 3,
                _ => 4
            };
        }
    }
}