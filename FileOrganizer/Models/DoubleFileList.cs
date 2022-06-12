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
                if (!File.Exists(f.FileInfo.FullName) && File.Exists($"{f.FileInfo.DirectoryName}\\{f.TentativeName}"))
                {
                    return;
                }
            }

            foreach (var file in ignoreFiles)
            {
                file.FileInfo.MoveTo($"{file.FileInfo.Directory}\\{file.TentativeName}");
            }
        }

        public void AppendNumber()
        {
            var number = 1;
            Files.ForEach(f =>
            {
                f.TentativeName = $"{ number++.ToString("0000")}_{f.Name}";
                if (!File.Exists(f.FileInfo.FullName) && File.Exists($"{f.FileInfo.DirectoryName}\\{f.TentativeName}"))
                {
                    return;
                }
            });

            Files.ForEach(f =>
            {
                f.FileInfo.MoveTo($"{f.FileInfo.Directory}\\{f.TentativeName}");
            });
        }

        public void AppendNumberWithoutIgnoreFile()
        {
            var withoutIgnoreFiles = Files.Where(f => !f.Ignore).ToList();
            var number = 1;

            withoutIgnoreFiles.ForEach(f =>
            {
                f.TentativeName = $"{ number++.ToString("0000")}_{f.Name}";
                if (!File.Exists(f.FileInfo.FullName) && File.Exists($"{f.FileInfo.DirectoryName}\\{f.TentativeName}"))
                {
                    return;
                }
            });

            withoutIgnoreFiles.ForEach(f =>
            {
                f.FileInfo.MoveTo($"{f.FileInfo.Directory}\\{f.TentativeName}");
            });
        }
    }
}
