using Minesweeper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Minesweeper.Converters
{
    public class TileToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string tileString = (string)value;

            ImageBrush retVal = new ImageBrush();

            //Display value
            switch (tileString) {

                case "mine":
                    retVal.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/mine.png"));
                    break;
                case "unopened":
                    retVal.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/unopened.svg"));
                    break;
                case "flag":
                    retVal.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/flag.svg"));
                    break;
                case "questionmark":
                    retVal.ImageSource = new BitmapImage(new Uri("ms-appx:///Images/questionmark.svg"));
                    break;
                default:
                    retVal.ImageSource = new BitmapImage(new Uri($"ms-appx:///Images/Minesweeper_{tileString}.svg"));
                    break;
        }
            return retVal;
            
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
