using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace WPF_SBAR {


    public class LastHeaderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class HeaderConverter : IValueConverter{

        Window window;

        public static HeaderConverter Instance = new HeaderConverter();
        //public static LastHeaderConverter Instance2 = new LastHeaderConverter();

        HeaderConverter() {
            window = (Application.Current.Windows[0] as MainWindow);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
           
            Uri uri;

            if ((value as string).Contains(@"\")){

                uri = new Uri("pack://application:,,,/Images/diskdrive.png");
                //uri = new Uri("file:///application:,,,");

            }//if
            else {
                uri = new Uri("pack://application:,,,/Images/folder.png");
            }//else

            BitmapImage source = new BitmapImage( uri );

            return source;

        }//Convert

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }//HeaderConverter

}
