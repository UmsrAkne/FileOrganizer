﻿namespace FileOrganizer.ViewModels
{
    using System.Collections.Generic;
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
        private DoubleFileList doubleFileList = new DoubleFileList(new List<ExtendFileInfo>());
        private bool ignoreFileIsVisible = true;

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

        public DelegateCommand ToggleIgnoreFileCommand => new DelegateCommand(() =>
        {
            if (SelectedItem != null)
            {
                SelectedItem.Ignore = !SelectedItem.Ignore;

                if (!ignoreFileIsVisible)
                {
                    var index = SelectedFileIndex; // Remove を行うとインデックスがリセットされるため変数に保持する。
                    ExtendFileInfos.Remove(SelectedItem);
                    SelectedFileIndex = index;
                }
            }

            ReIndex();
        });

        public DelegateCommand DisplayIgnoreFileCommand => new DelegateCommand(() =>
        {
            ExtendFileInfos = new ObservableCollection<ExtendFileInfo>(doubleFileList.GetFiles());
            ignoreFileIsVisible = true;
        });

        public DelegateCommand HideIgnoreFileCommand => new DelegateCommand(() =>
        {
            ExtendFileInfos = new ObservableCollection<ExtendFileInfo>(doubleFileList.GetExceptedIgnoreFiles());
            ignoreFileIsVisible = false;
        });

        public DelegateCommand AppendPrefixToIgnoreFilesCommand => new DelegateCommand(() =>
        {
            doubleFileList.AppendPrefixToIgnoreFiles("ignore");
        });

        // 基本的にビヘイビアから呼び出される
        public void SetFiles(List<ExtendFileInfo> files)
        {
            ExtendFileInfos = new ObservableCollection<ExtendFileInfo>(files);
            doubleFileList = new DoubleFileList(files);
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
        }
    }
}
