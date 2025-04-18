using FuzzySharp.Extractor;
using FuzzySharp.SimilarityRatio.Scorer;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenSettings.Docs
{
    public class FallbackPageMiddleware(RequestDelegate next, StaticDocsFileMap staticDocsFileMap, PageNotFound pageNotFound)
    {
        private const string TextHtmlContentType = "text/html;charset=utf-8";

        private readonly IRatioScorer _scorer = null;

        public async Task InvokeAsync(HttpContext httpContext)
        {
            await next(httpContext);

            if (httpContext.Response.StatusCode != 404)
            {
                return;
            }

            var requestInfo = new RequestInfo(httpContext, staticDocsFileMap);

            if (!requestInfo.HasRequestPath)
            {
                httpContext.Response.Redirect(staticDocsFileMap.LatestVersion.IndexHtml);
                return;
            }

            string pageNotFoundFilePath;
            IEnumerable<ExtractedResult<string>> extractedResults;

            if (staticDocsFileMap.MajorVersionToDocSet.TryGetValue(requestInfo.MajorVersion, out var docSet))
            {
                pageNotFoundFilePath = docSet.PageNotFoundFilePath;
                extractedResults = FuzzySharp.Process.ExtractTop(requestInfo.RequestPath, docSet.RelativeHtmlPaths, scorer: _scorer);
            }
            else
            {
                docSet = staticDocsFileMap.MajorVersionToDocSet.First().Value;

                pageNotFoundFilePath = docSet.PageNotFoundFilePath;
                extractedResults = FuzzySharp.Process.ExtractTop(requestInfo.RequestPath, staticDocsFileMap.RelativeHtmlPaths, scorer: _scorer);
            }

            var extractedResultsAsArray = extractedResults.OrderByDescending(e => e.Score).ThenBy(e => e.Value).ToArray();

            if (extractedResultsAsArray.Length > 0)
            {
                var extractedResult = extractedResultsAsArray[0];

                if (extractedResult.Score > 75)
                {
                    var redirect = $"{requestInfo.RequestPath}.html" == extractedResult.Value ? extractedResult.Value : $"{extractedResult.Value}?originalPath={requestInfo.RequestPath}";
                    httpContext.Response.Redirect(redirect);
                    return;
                }
            }

            if (!requestInfo.HasMajorVersion)
            {
                httpContext.Response.Redirect($"/{docSet.MajorVersion}{requestInfo.RequestPath.TrimEnd('/')}");
                return;
            }

            if (requestInfo.RequestPath.EndsWith('/'))
            {
                httpContext.Response.Redirect(requestInfo.RequestPath.TrimEnd(('/')));
                return;
            }

            var htmlContent = await pageNotFound.GetHtmlContentAsync(pageNotFoundFilePath, extractedResultsAsArray.Where(e => e.Score > 35).Select(e => e.Value), httpContext.RequestAborted);

            await RespondWithHtmlContentAsync(httpContext.Response, htmlContent);
        }

        private static Task RespondWithHtmlContentAsync(HttpResponse response, string text)
        {
            response.StatusCode = 200;
            response.ContentType = TextHtmlContentType;

            return response.WriteAsync(text, Encoding.UTF8, response.HttpContext.RequestAborted);
        }
    }
}