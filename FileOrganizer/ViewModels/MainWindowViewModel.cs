namespace FileOrganizer.ViewModels
{
    using System.Collections.ObjectModel;
    using FileOrganizer.Models;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private ObservableCollection<ExtendFileInfo> extendFileInfos;

        public MainWindowViewModel()
        {
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        // 基本的にビヘイビアから入力される。
        public ObservableCollection<ExtendFileInfo> ExtendFileInfos
        {
            get => extendFileInfos;
            set => SetProperty(ref extendFileInfos, value);
        }
    }
}
