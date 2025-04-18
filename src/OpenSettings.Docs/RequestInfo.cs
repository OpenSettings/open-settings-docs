using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace OpenSettings.Docs
{
    public class RequestInfo
    {
        private const string SlashString = "/";
        private const char SlashChar = '/';

        public RequestInfo(HttpContext httpContext, StaticDocsFileMap staticDocsFileMap)
        {
            if (!httpContext.Request.Path.HasValue || httpContext.Request.Path.Value == SlashString)
            {
                return;
            }

            HasRequestPath = true;
            RequestPath = httpContext.Request.Path.Value;
            MajorVersion = RequestPath.Split(SlashChar).Skip(1).First();
            HasMajorVersion = staticDocsFileMap.MajorVersionToDocSet.ContainsKey(MajorVersion.ToLowerInvariant());
            Extension = Path.GetExtension(RequestPath);
            ExtensionWithoutDot = Extension.Length > 0 ? Extension[1..] : string.Empty;
            HasExtension = !string.IsNullOrWhiteSpace(ExtensionWithoutDot) && !int.TryParse(ExtensionWithoutDot, out _);
        }

        public bool HasRequestPath { get; }

        public string RequestPath { get; }

        public string MajorVersion { get; }

        public bool HasMajorVersion { get; }

        public string Extension { get; }

        public string ExtensionWithoutDot { get; }

        public bool HasExtension { get; }
    }
}