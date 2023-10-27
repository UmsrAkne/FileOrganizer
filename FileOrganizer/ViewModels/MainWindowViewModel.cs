using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using FileOrganizer.Models;
using Prism.Commands;
using Prism.Mvvm;
using WMPLib;

namespace FileOrganizer.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowViewModel : BindableBase
    {
        private readonly WindowsMediaPlayer windowsMediaPlayer = new WindowsMediaPlayer();

        private readonly Renamer renamer = new Renamer();

        private string title = "File Organizer";
        private ObservableCollection<ExtendFileInfo> extendFileInfos = new ObservableCollection<ExtendFileInfo>();
        private ExtendFileInfo selectedItem;
        private int selectedFileIndex;
        private bool ignoreFileIsVisible = true;
        private int ignoreFileCount;
        private int maximumIndex;
        private int markedFileCount;
        private double fontSize = 12.0;

        public string Title { get => title; set => SetProperty(ref title, value); }

        public FileContainer FileContainer { get; set; }

        public ObservableCollection<ExtendFileInfo> ExtendFileInfos
        {
            get => extendFileInfos;
            private set => SetProperty(ref extendFileInfos, value);
        }

        public ExtendFileInfo SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        public int SelectedFileIndex { get => selectedFileIndex; set => SetProperty(ref selectedFileIndex, value); }

        public int MaximumIndex { get => maximumIndex; set => SetProperty(ref maximumIndex, value); }

        public double FontSize { get => fontSize; set => SetProperty(ref fontSize, value); }

        public DelegateCommand<object> SetFontSizeCommand => new DelegateCommand<object>((size) =>
        {
            FontSize = (double)size;
        });

        public DelegateCommand ToggleMarkCommand => new DelegateCommand(() =>
        {
            if (SelectedItem != null)
            {
                SelectedItem.Marked = !SelectedItem.Marked;
                MarkedFileCount += SelectedItem.Marked ? 1 : -1;
            }
        });

        public DelegateCommand JumpToNextMarkCommand => new DelegateCommand(() =>
        {
            var nextMark = ExtendFileInfos.Skip(SelectedFileIndex + 1).FirstOrDefault(f => f.Marked);

            if (nextMark != null)
            {
                SelectedFileIndex = ExtendFileInfos.IndexOf(nextMark);
            }
        });

        public DelegateCommand JumpToPrevMarkCommand => new DelegateCommand(() =>
        {
            var prevMark = ExtendFileInfos.Take(SelectedFileIndex - 1).Reverse().FirstOrDefault(f => f.Marked);

            if (prevMark != null)
            {
                SelectedFileIndex = ExtendFileInfos.IndexOf(prevMark);
            }
        });

        public DelegateCommand DisplayIgnoreFileCommand => new DelegateCommand(() =>
        {
            IgnoreFileIsVisible = true;
        });

        public DelegateCommand HideIgnoreFileCommand => new DelegateCommand(() =>
        {
            IgnoreFileIsVisible = false;
        });

        public DelegateCommand AppendPrefixToIgnoreFilesCommand => new DelegateCommand(() =>
        {
            var ignoreFiles = ExtendFileInfos.Where(f => f.Ignore);
            renamer.AppendPrefix("ignore_", ignoreFiles);
        });

        public DelegateCommand AppendNumberCommand => new DelegateCommand(() =>
        {
            renamer.AppendNumber(ExtendFileInfos);
        });

        public DelegateCommand AppendNumberWithoutIgnoreFileCommand => new DelegateCommand(() =>
        {
            var files = ExtendFileInfos.Where(f => !f.Ignore);
            renamer.AppendNumber(files);
        });

        public DelegateCommand ReverseCommand => new DelegateCommand(() =>
        {
            ExtendFileInfos = new ObservableCollection<ExtendFileInfo>(ExtendFileInfos.Reverse());
            ReIndex();
        });

        public DelegateCommand PlaySoundCommand => new DelegateCommand(() =>
        {
            if (SelectedItem == null)
            {
                return;
            }

            if (SelectedItem.IsSoundFile)
            {
                ExtendFileInfos.ToList().ForEach(f => f.Playing = false);
                SelectedItem.Playing = true;
                windowsMediaPlayer.URL = SelectedItem.FileInfo.FullName;
                windowsMediaPlayer.controls.play();
                return;
            }

            Process.Start(SelectedItem.FileInfo.FullName);
        });

        public bool IgnoreFileIsVisible { get => ignoreFileIsVisible; set => SetProperty(ref ignoreFileIsVisible, value); }

        public int IgnoreFileCount { get => ignoreFileCount; set => SetProperty(ref ignoreFileCount, value); }

        public int MarkedFileCount { get => markedFileCount; set => SetProperty(ref markedFileCount, value); }

        private int ListViewItemLineHeight => 15;

        // 基本的にビヘイビアから呼び出される
        public void SetFiles(List<ExtendFileInfo> files)
        {
            FileContainer = new FileContainer(files);
            RaisePropertyChanged(nameof(FileContainer));

            MarkedFileCount = 0;
            IgnoreFileCount = 0;
        }

        private void ReIndex()
        {
            var index = 1;

            foreach (var f in ExtendFileInfos)
            {
                f.Index = f.Ignore ? 0 : index++;
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