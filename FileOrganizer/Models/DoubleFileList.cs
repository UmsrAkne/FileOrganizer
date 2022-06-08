namespace FileOrganizer.Models
{
    using System.Collections.Generic;
    using System.IO;
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

        public void AppendPrefixToIgnoreFiles(string prefix)
        {
            var ignoreFiles = Files.Where(f => f.Ignore).ToList();
            for (int i = 0; i < ignoreFiles.Count; i++)
            {
                var f = ignoreFiles[i];
                f.TentativeName = $"{prefix}_{f.Name}";
                if (File.Exists($"{f.FileInfo.DirectoryName}\\{f.TentativeName}"))
                {
                    return;
                }
            }

            foreach (var file in ignoreFiles)
            {
                file.FileInfo.MoveTo($"{file.FileInfo.Directory}\\{file.TentativeName}");
            }
        }
    }
}
