namespace FileOrganizer.Models
{
    using System.IO;
    using Prism.Mvvm;

    public class ExtendFileInfo : BindableBase
    {
        private bool isSelected;
        private bool ignore;
        private int index;

        public ExtendFileInfo(string path)
        {
            FileInfo = new FileInfo(path);
        }

        public FileInfo FileInfo { get; private set; }

        public string Name => FileInfo.Name;

        public bool Ignore { get => ignore; set => SetProperty(ref ignore, value); }

        public int Index { get => index; set => SetProperty(ref index, value); }

        public string TentativeName { get; set; }
    }
}
