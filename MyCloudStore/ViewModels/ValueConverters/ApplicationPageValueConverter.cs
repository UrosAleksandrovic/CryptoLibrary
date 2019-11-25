using System;
using System.Diagnostics;
using System.Globalization;
using MyCloudStore.Models;
using MyCloudStore.Pages;

namespace MyCloudStore.ViewModels
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an view/page
    /// </summary>
    class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch((ApplicationPage)value)
            {
                case ApplicationPage.Login:
                    return new LoginPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
