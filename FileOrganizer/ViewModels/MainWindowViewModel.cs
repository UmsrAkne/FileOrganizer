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
        private int maximumIndex;
        private double fontSize = 12.0;
        private int listViewItemLineHeight = 15;

        public string Title { get => title; set => SetProperty(ref title, value); }

        public FileContainer FileContainer { get; set; }

        public ObservableCollection<ExtendFileInfo> ExtendFileInfos
        {
            get => extendFileInfos;
            private set => SetProperty(ref extendFileInfos, value);
        }

        public ExtendFileInfo SelectedItem { get => selectedItem; set => SetProperty(ref selectedItem, value); }

        public int MaximumIndex { get => maximumIndex; private set => SetProperty(ref maximumIndex, value); }

        public double FontSize { get => fontSize; private set => SetProperty(ref fontSize, value); }

        public DelegateCommand<object> SetFontSizeCommand => new DelegateCommand<object>((size) =>
        {
            FontSize = (double)size;
            ListViewItemLineHeight = (int)((double)size * 1.25);
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

        public DelegateCommand<object> PageDownCommand => new DelegateCommand<object>((lvActualHeight) =>
        {
            FileContainer.MoveCursorCommand.Execute(GetDisplayingItemCount((double)lvActualHeight));
        });

        public DelegateCommand<object> PageUpCommand => new DelegateCommand<object>((lvActualHeight) =>
        {
            FileContainer.MoveCursorCommand.Execute(GetDisplayingItemCount((double)lvActualHeight) * -1);
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

        public int ListViewItemLineHeight
        {
            get => listViewItemLineHeight;
            private set => SetProperty(ref listViewItemLineHeight, value);
        }

        // 基本的にビヘイビアから呼び出される
        public void SetFiles(List<ExtendFileInfo> files)
        {
            FileContainer = new FileContainer(files);
            RaisePropertyChanged(nameof(FileContainer));
        }

        private int GetDisplayingItemCount(double lvActualHeight)
        {
            // + 5 はボーダー等によるズレの補正値。厳密に正確な表示数が出るわけではない。大体当たっている程度。
            return (int)Math.Floor(lvActualHeight / (ListViewItemLineHeight + 5));
        }
    }
}