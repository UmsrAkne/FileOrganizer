using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Commands;
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

            if (Files.Count > 0)
            {
                CursorIndex = 0;
            }
        }

        private List<ExtendFileInfo> OriginalFiles { get; set; }

        public List<ExtendFileInfo> Files { get => files; set => SetProperty(ref files, value); }

        public int CursorIndex
        {
            get => cursorIndex;
            private set
            {
                if (Files == null || Files.Count == 0)
                {
                    return;
                }

                if (cursorIndex + value < 0)
                {
                    SetProperty(ref cursorIndex, 0);
                    return;
                }

                if (cursorIndex + value >= Files.Count)
                {
                    SetProperty(ref cursorIndex, Files.Count - 1);
                    return;
                }

                SetProperty(ref cursorIndex, value);
            }
        }

        public DelegateCommand<object> MoveCursorCommand => new DelegateCommand<object>((count) =>
        {
            CursorIndex += (int)count;
        });
    }
}