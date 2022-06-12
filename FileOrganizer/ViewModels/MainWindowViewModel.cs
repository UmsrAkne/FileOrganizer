namespace FileOrganizer.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Controls;
    using FileOrganizer.Models;
    using Prism.Commands;
    using Prism.Mvvm;
    using WMPLib;

    public class MainWindowViewModel : BindableBase
    {
        private string title = "Prism Application";
        private ObservableCollection<ExtendFileInfo> extendFileInfos = new ObservableCollection<ExtendFileInfo>();
        private ExtendFileInfo selectedItem;
        private int selectedFileIndex;
        private DoubleFileList doubleFileList = new DoubleFileList(new List<ExtendFileInfo>());
        private bool ignoreFileIsVisible = true;
        private WindowsMediaPlayer windowsMediaPlayer = new WindowsMediaPlayer();
        private int ignoreFileCount;
        private int maximumIndex;
        private int markedFileCount;

        public MainWindowViewModel()
        {
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ObservableCollection<ExtendFileInfo> ExtendFileInfos
        {
            get => extendFileInfos;
            set => SetProperty(ref extendFileInfos, value);
        }

        public ExtendFileInfo SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        public int SelectedFileIndex { get => selectedFileIndex; set => SetProperty(ref selectedFileIndex, value); }

        public bool IgnoreFileIsVisible { get => ignoreFileIsVisible; set => SetProperty(ref ignoreFileIsVisible, value); }

        public int IgnoreFileCount { get => ignoreFileCount; set => SetProperty(ref ignoreFileCount, value); }

        public int MaximumIndex { get => maximumIndex; set => SetProperty(ref maximumIndex, value); }

        public int MarkedFileCount { get => markedFileCount; set => SetProperty(ref markedFileCount, value); }

        public int ListViewItemLineHeight => 15;

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

        public DelegateCommand<ListView> CursorPageUpCommand => new DelegateCommand<ListView>((lv) =>
        {
            var command = CursorUpCommand;
            Enumerable.Range(0, GetDisplayingItemCount(lv)).ToList().ForEach(i => command.Execute(lv));
        });

        public DelegateCommand<ListView> CursorPageDownCommand => new DelegateCommand<ListView>((lv) =>
        {
            var command = CursorDownCommand;
            Enumerable.Range(0, GetDisplayingItemCount(lv)).ToList().ForEach(i => command.Execute(lv));
        });

        public DelegateCommand ToggleIgnoreFileCommand => new DelegateCommand(() =>
        {
            if (SelectedItem != null)
            {
                SelectedItem.Ignore = !SelectedItem.Ignore;
                IgnoreFileCount += SelectedItem.Ignore ? 1 : -1;

                if (!IgnoreFileIsVisible)
                {
                    var index = SelectedFileIndex; // Remove を行うとインデックスがリセットされるため変数に保持する。
                    ExtendFileInfos.Remove(SelectedItem);
                    SelectedFileIndex = index;
                }
            }

            ReIndex();
        });

        public DelegateCommand ToggleMarkCommand => new DelegateCommand(() =>
        {
            if (SelectedItem != null)
            {
                SelectedItem.Marked = !SelectedItem.Marked;
                MarkedFileCount += SelectedItem.Marked ? 1 : -1;
            }
        });

        public DelegateCommand<ListView> JumpToNextMarkCommand => new DelegateCommand<ListView>((lv) =>
        {
            var nextMark = ExtendFileInfos.Skip(SelectedFileIndex + 1).FirstOrDefault(f => f.Marked);

            if (nextMark != null)
            {
                lv.ScrollIntoView(nextMark);
                SelectedFileIndex = ExtendFileInfos.IndexOf(nextMark);
            }
        });

        public DelegateCommand<ListView> JumpToPrevMarkCommand => new DelegateCommand<ListView>((lv) =>
        {
            var prevMark = ExtendFileInfos.Take(SelectedFileIndex - 1).Reverse().FirstOrDefault(f => f.Marked);

            if (prevMark != null)
            {
                lv.ScrollIntoView(prevMark);
                SelectedFileIndex = ExtendFileInfos.IndexOf(prevMark);
            }
        });

        public DelegateCommand DisplayIgnoreFileCommand => new DelegateCommand(() =>
        {
            IgnoreFileIsVisible = true;
            ReloadCommand.Execute();
        });

        public DelegateCommand HideIgnoreFileCommand => new DelegateCommand(() =>
        {
            IgnoreFileIsVisible = false;
            ReloadCommand.Execute();
        });

        public DelegateCommand AppendPrefixToIgnoreFilesCommand => new DelegateCommand(() =>
        {
            doubleFileList.AppendPrefixToIgnoreFiles("ignore");
            ReloadCommand.Execute();
        });

        public DelegateCommand AppendNumberCommand => new DelegateCommand(() =>
        {
            doubleFileList.AppendNumber();
            ReloadCommand.Execute();
        });

        public DelegateCommand AppendNumberWithoutIgnoreFileCommand => new DelegateCommand(() =>
        {
            doubleFileList.AppendNumberWithoutIgnoreFile();
            ReloadCommand.Execute();
        });


        public DelegateCommand ReloadCommand => new DelegateCommand(() =>
        {
            if (IgnoreFileIsVisible)
            {
                ExtendFileInfos = new ObservableCollection<ExtendFileInfo>(doubleFileList.GetFiles());
            }
            else
            {
                ExtendFileInfos = new ObservableCollection<ExtendFileInfo>(doubleFileList.GetExceptedIgnoreFiles());
            }
        });

        public DelegateCommand PlaySoundCommand => new DelegateCommand(() =>
        {
            if (SelectedItem != null || SelectedItem.IsSoundFile)
            {
                doubleFileList.GetFiles().ForEach(f => f.Playing = false);
                SelectedItem.Playing = true;
                windowsMediaPlayer.URL = SelectedItem.FileInfo.FullName;
                windowsMediaPlayer.controls.play();
            }
        });

        // 基本的にビヘイビアから呼び出される
        public void SetFiles(List<ExtendFileInfo> files)
        {
            ExtendFileInfos = new ObservableCollection<ExtendFileInfo>(files);
            doubleFileList = new DoubleFileList(files);

            MarkedFileCount = 0;
            IgnoreFileCount = 0;

            ReIndex();
        }

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

            MaximumIndex = index - 1;
        }

        private int GetDisplayingItemCount(ListView lv)
        {
            // + 5 はボーダー等によるズレの補正値。厳密に正確な表示数が出るわけではない。大体当たっている程度。
            return (int)Math.Floor(lv.ActualHeight / (ListViewItemLineHeight + 5));
        }
    }
}
