using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace FileOrganizer.Models
{
    public class FileContainer : BindableBase
    {
        private List<ExtendFileInfo> files;
        private int cursorIndex = -1;

        public FileContainer(IEnumerable<ExtendFileInfo> extendFileInfos)
        {
            OriginalFiles = extendFileInfos.ToList();
            Files = OriginalFiles.OrderBy(f => f.Name).ToList();
        }

        private List<ExtendFileInfo> OriginalFiles { get; set; }

        public List<ExtendFileInfo> Files { get => files; set => SetProperty(ref files, value); }

        public int CursorIndex
        {
            get => cursorIndex;
            set
            {
                if (Files == null || Files.Count == 0)
                {
                    return;
                }

                if (value < 0 || value > Files.Count)
                {
                    value = Math.Max(0, value);
                    value = Math.Min(Files.Count - 1, value);
                }

                SetProperty(ref cursorIndex, value);
            }
        }
    }
}