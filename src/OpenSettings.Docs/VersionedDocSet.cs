namespace OpenSettings.Docs
{
    public class VersionedDocSet
    {
        public string MajorVersion { get; set; }

        public string IndexHtml { get; set; }

        public string IndexHtmlFilePath { get; set; }

        public string PageNotFound { get; set; }

        public string PageNotFoundFilePath { get; set; }

        public DocFileEntry[] Files { get; set; }

        public string[] RelativePathsWithExtension { get; set; }

        public string[] RelativeHtmlPaths { get; set; }
    }
}