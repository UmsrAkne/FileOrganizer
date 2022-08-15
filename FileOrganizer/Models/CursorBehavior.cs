namespace FileOrganizer.Models
{
    using Microsoft.Xaml.Behaviors;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public class CursorBehavior : Behavior<ListView>
    {
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(CursorBehavior), new UIPropertyMetadata(null));

        // 要素にアタッチされたときの処理。大体イベントハンドラの登録処理をここでやる
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.KeyDown += AssociatedObject_KeyDown;
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            var lv = sender as ListView;

            if (e.Key == Key.J)
            {
                lv.SelectedIndex++;
                lv.ScrollIntoView(lv.SelectedItem);
            }

            if (e.Key == Key.K)
            {
                if (lv.SelectedIndex >= 1)
                {
                    lv.SelectedIndex--;
                    lv.ScrollIntoView(lv.SelectedItem);
                }
            }
        }

        // 要素にデタッチされたときの処理。大体イベントハンドラの登録解除をここでやる
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
        }
    }
}
