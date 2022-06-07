namespace FileOrganizer.ViewModels
{
    using System.Collections.ObjectModel;
    using FileOrganizer.Models;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";

        public MainWindowViewModel()
        {
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        // 基本的にビヘイビアから入力される。
        public ObservableCollection<ExtendFileInfo> ExtendFileInfos { get; set; }
    }
}
