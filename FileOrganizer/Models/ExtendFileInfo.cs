namespace FileOrganizer.Models
{
    using System.IO;

    public class ExtendFileInfo
    {
        public ExtendFileInfo(string path)
        {
            FileInfo = new FileInfo(path);
        }

        public FileInfo FileInfo { get; private set; }

        public string Name => FileInfo.Name;

        public int Index { get; set; }
    }
}
