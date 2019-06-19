using Execucao;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Modelagem
{
    [ValueConversion(typeof(ErroTipo), typeof(string))]
    public class IconeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ErroTipo)value)
            {
                case ErroTipo.Aviso:
                    return "resources/aviso.png";
                case ErroTipo.Erro:
                    return "resources/erro.png";
                case ErroTipo.Informacao:
                    return "resources/info.png";
                default:
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
