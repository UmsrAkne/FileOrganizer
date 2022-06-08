namespace FileOrganizer.Models
{
    using System.IO;
    using Prism.Mvvm;

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

    }
}
