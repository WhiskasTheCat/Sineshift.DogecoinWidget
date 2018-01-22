using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.Globalization;

namespace Sineshift.DogecoinWidget.Common.UI
{
    public class MathConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || parameter.ToString().Length <= 1)
                return value;

            double invalue = double.Parse(value.ToString());
            string param = parameter.ToString();

            string op = param[0].ToString();
            double num = double.Parse(param.Substring(1));

            switch (op)
            {
                case "+":
                    return invalue + num;
                case "-":
                    return invalue - num;
                case "*":
                    return invalue * num;
                case "/":
                    return invalue / num;
            }

            return invalue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
