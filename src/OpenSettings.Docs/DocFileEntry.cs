namespace OpenSettings.Docs
{
    public class DocFileEntry
    {
        public string FullPath { get; set; }

        public string RelativePath { get; set; }

        public string RelativePathWithExtension { get; set; }

        public string FileName { get; set; }

        public string Extension { get; set; }

        public bool HasExtension { get; set; }

        public bool IsHtmlFile { get; set; }
    }
}