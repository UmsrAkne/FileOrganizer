using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileOrganizer.Models
{
    // ReSharper disable once IdentifierTypo
    public class Renamer
    {
        public void AppendPrefix(string prefix, IEnumerable<ExtendFileInfo> files)
        {
            var extendFileInfos = files.ToList();
            foreach (var f in extendFileInfos)
            {
                f.TentativeName = $"{prefix}_{f.Name}";
                if (!File.Exists(f.FileInfo.FullName) && File.Exists($"{f.FileInfo.DirectoryName}\\{f.TentativeName}"))
                {
                    return;
                }
            }

            foreach (var f in extendFileInfos)
            {
                f.FileInfo.MoveTo($"{f.FileInfo.Directory}\\{f.TentativeName}");
            }
        }

        public void AppendNumber(IEnumerable<ExtendFileInfo> files)
        {
            var number = 0;
            var extendFileInfos = files.ToList();
            var occuredError = false;

            extendFileInfos.ForEach(f =>
            {
                number++;
                var numberString = number.ToString("0000");
                f.TentativeName = $"{numberString}_{f.Name}";

                if (!File.Exists(f.FileInfo.FullName) && File.Exists($"{f.FileInfo.DirectoryName}\\{f.TentativeName}"))
                {
                    occuredError = true;
                }
            });

            if (occuredError)
            {
                return;
            }

            extendFileInfos.ForEach(f =>
            {
                f.FileInfo.MoveTo($"{f.FileInfo.Directory}\\{f.TentativeName}");
            });
        }
    }
}