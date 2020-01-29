using Microsoft.AspNetCore.Razor.Language;

namespace BlazorBolt
{
    public class FileState
    {
        public FileState(string fileName)
        {
            FileName = fileName;
            FilePath = "/" + FileName;
        }

        public string FileName { get; }

        public string FilePath { get; }

        public string Content { get; set; }

        public RazorCodeDocument Generated { get; set; }
    }
}