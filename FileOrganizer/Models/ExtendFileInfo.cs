namespace FileOrganizer.Models
{
    using System.IO;
    using Prism.Mvvm;

    public class ExtendFileInfo : BindableBase
    {
        private bool isSelected;
        private bool ignore;
        private int index;
        private bool playing;
        private bool marked;

        public ExtendFileInfo(string path)
        {
            FileInfo = new FileInfo(path);
        }

        public FileInfo FileInfo { get; private set; }

        public string Name => FileInfo.Name;

        public bool Ignore { get => ignore; set => SetProperty(ref ignore, value); }

        public int Index { get => index; set => SetProperty(ref index, value); }

        public string TentativeName { get; set; }

        public bool IsSoundFile => FileInfo.Extension == "mp3" || FileInfo.Extension == "ogg" || FileInfo.Extension == "wma";

        public bool Playing { get => playing; set => SetProperty(ref playing, value); }

        public bool Marked { get => marked; set => SetProperty(ref marked, value); }
    }
}
