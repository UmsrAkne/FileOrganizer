namespace FileOrganizer.Models
{
    using System.Collections.Generic;
    using System.Linq;

    public class DoubleFileList
    {
        public DoubleFileList(List<ExtendFileInfo> files)
        {
            Files = new List<ExtendFileInfo>(files);
            OriginalFiles = Files.ToList();
        }

        public List<ExtendFileInfo> Files { get; set; }

        private List<ExtendFileInfo> OriginalFiles { get; set; }

        public List<ExtendFileInfo> GetExceptedIgnoreFiles()
        {
            return Files.Where(f => !f.Ignore).ToList();
        }

        public List<ExtendFileInfo> GetFiles()
        {
            return OriginalFiles.ToList();
        }
    }
}
