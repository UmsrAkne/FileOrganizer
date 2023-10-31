namespace FileOrganizer.Models
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    public class NumberFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                // "0" を指定回数分詰め込んだ配列を作成
                var zeros = Enumerable.Range(1, int.Parse((string)parameter)).Select(n => "0").ToArray();
                return string.Format("{0:" + string.Join(string.Empty, zeros) + "}", (int)value);
            }

            return value != null ? $"{(int)value:0000}" : "0000";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}