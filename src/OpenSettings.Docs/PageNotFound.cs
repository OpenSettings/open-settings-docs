using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenSettings.Docs
{
    public class PageNotFound
    {
        private const string SuggestionTextReplacementKey = "%(Suggestion)";
        private const string SuggestionsFormat = """
                                         <br>
                                         <h2 id="suggestions">
                                           👇 Could this be the page you're looking for?
                                         </h2>
                                         <ul>
                                         {0}
                                         </ul>
                                         <br>
                                         """;

        public async Task<string> GetHtmlContentAsync(string pageNotFoundFilePath, IEnumerable<string> suggestions, CancellationToken cancellationToken = default)
        {
            using var pool = new StringBuilderPool();

            foreach (var suggestion in suggestions)
            {
                pool.Builder.Append("<li><a href=\"")
                    .Append(suggestion)
                    .Append("\">")
                    .Append(suggestion)
                    .Append("</a></li>")
                    .AppendLine();
            }

            using (var fileStream = File.OpenRead(pageNotFoundFilePath))
            {
                using (var reader = new StreamReader(fileStream))
                {
                    var htmlBuilder = new StringBuilder(await reader.ReadToEndAsync(cancellationToken));

                    htmlBuilder = htmlBuilder.Replace(SuggestionTextReplacementKey, pool.Builder.Length == 0 ? string.Empty : string.Format(SuggestionsFormat, pool.Builder));

                    return htmlBuilder.ToString();
                }
            }
        }
    }
}