namespace FileOrganizer.Models
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using FileOrganizer.ViewModels;
    using Microsoft.Xaml.Behaviors;

    public class DropBehavior : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            // ファイルをドラッグしてきて、コントロール上に乗せた際の処理
            AssociatedObject.PreviewDragOver += this.AssociatedObject_PreviewDragOver;

            // ファイルをドロップした際の処理
            AssociatedObject.Drop += this.AssociatedObject_Drop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewDragOver -= this.AssociatedObject_PreviewDragOver;
            AssociatedObject.Drop -= this.AssociatedObject_Drop;
        }

        private void AssociatedObject_Drop(object sender, DragEventArgs e)
        {
            // ファイルパスの一覧の配列
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            var fileCollection = new ObservableCollection<ExtendFileInfo>(files.Select(p => new ExtendFileInfo(p)).ToList());
            ((sender as Window).DataContext as MainWindowViewModel).ExtendFileInfos = fileCollection;
        }

        private void AssociatedObject_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
        }
    }
}
