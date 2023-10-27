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
        private bool containsIgnoreFiles = true;
        private bool isReverseOrder;

        public FileContainer(IEnumerable<ExtendFileInfo> extendFileInfos)
        {
            OriginalFiles = extendFileInfos.ToList();
            ReloadCommand.Execute();

            if (Files.Count > 0)
            {
                CursorIndex = 0;
            }
        }

        public List<ExtendFileInfo> Files { get => files; private set => SetProperty(ref files, value); }

        public bool ContainsIgnoreFiles
        {
            get => containsIgnoreFiles;
            set
            {
                if (SetProperty(ref containsIgnoreFiles, value))
                {
                    ReloadCommand.Execute();
                }
            }
        }

        public bool IsReverseOrder { get => isReverseOrder; set => SetProperty(ref isReverseOrder, value); }

        public int StartIndex { get; set; } = 1;

        public int CursorIndex
        {
            get => cursorIndex;
            set
            {
                if (Files == null || Files.Count == 0)
                {
                    SetProperty(ref cursorIndex, -1);
                    return;
                }

                if (value < 0)
                {
                    SetProperty(ref cursorIndex, 0);
                    return;
                }

                if (value >= Files.Count)
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

        public DelegateCommand JumpToNextMarkedFileCommand => new DelegateCommand(() =>
        {
            var nextMark = Files.Skip(CursorIndex + 1).FirstOrDefault(f => f.Marked);

            if (nextMark != null)
            {
                CursorIndex = Files.IndexOf(nextMark);
            }
        });

        public DelegateCommand JumpToPrevMarkedFileCommand => new DelegateCommand(() =>
        {
            var prevMark = Files.Take(CursorIndex).Reverse().FirstOrDefault(f => f.Marked);

            if (prevMark != null)
            {
                CursorIndex = Files.IndexOf(prevMark);
            }
        });

        public DelegateCommand ReloadCommand => new DelegateCommand(() =>
        {
            Files = OriginalFiles
                .Where(f => ContainsIgnoreFiles || !f.Ignore)
                .OrderBy(f => f.Name).ToList();

            if (IsReverseOrder)
            {
                Files.Reverse();
            }

            var index = StartIndex;

            foreach (var f in Files.Where(f => !f.Ignore))
            {
                f.Index = index++;
            }

            CursorIndex = CursorIndex;
        });

        private List<ExtendFileInfo> OriginalFiles { get; set; }
    }
}