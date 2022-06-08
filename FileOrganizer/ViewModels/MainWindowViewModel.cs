namespace FileOrganizer.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Windows.Controls;
    using FileOrganizer.Models;
    using Prism.Commands;
    using Prism.Mvvm;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private ObservableCollection<ExtendFileInfo> extendFileInfos;
        private ExtendFileInfo selectedItem;
        private int selectedFileIndex;

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

        public ExtendFileInfo SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        public int SelectedFileIndex { get => selectedFileIndex; set => SetProperty(ref selectedFileIndex, value); }

        public DelegateCommand<ListView> CursorUpCommand => new DelegateCommand<ListView>(lv =>
        {
            if (SelectedFileIndex > 0)
            {
                SelectedFileIndex--;
                lv.ScrollIntoView(SelectedItem);
            }
        });

        public DelegateCommand<ListView> CursorDownCommand => new DelegateCommand<ListView>(lv =>
        {
            if (SelectedFileIndex < ExtendFileInfos.Count - 1)
            {
                SelectedFileIndex++;
                lv.ScrollIntoView(SelectedItem);
            }
        });

        public DelegateCommand IgnoreFileCommand => new DelegateCommand(() =>
        {
            if (SelectedItem != null)
            {
                SelectedItem.Ignore = true;
            }

            ReIndex();
        });

        private void ReIndex()
        {
            var index = 1;

            foreach (var f in ExtendFileInfos)
            {
                if (f.Ignore)
                {
                    f.Index = 0;
                }
                else
                {
                    f.Index = index++;
                }
            }
        }
    }
}
