using MaterialDesignThemes.Wpf;
using RoverChallenge.Enums;
using System;
using System.Globalization;
using System.Windows.Data;

namespace RoverChallenge.Converters;

public class ViewToPackIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ApplicationViews applicationView)
        {
            switch (applicationView)
            {
                case ApplicationViews.None:
                    return new PackIcon() { Kind = PackIconKind.Error };

                case ApplicationViews.SettingsView:
                    return new PackIcon() { Kind = PackIconKind.Cog };

                case ApplicationViews.HelpView:
                    return new PackIcon() { Kind = PackIconKind.Help };

                case ApplicationViews.MapView:
                    return new PackIcon() { Kind = PackIconKind.Home };

                default:
                    return new PackIcon() { Kind = PackIconKind.Error };
            }
        }

        return new PackIcon() { Kind = PackIconKind.Error };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

