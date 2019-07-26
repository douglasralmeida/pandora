using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Base
{
    public enum AtividadeFase
    {
        [Description("Execução")] 
        FaseNormal,
        [Description("Pré-execução")]
        FasePre,
        [Description("Pós-execução")]
        FasePos
    }

    public class AtividadeFaseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumVal = (AtividadeFase)value;
            switch (enumVal)
            {
                case AtividadeFase.FasePre: return "Pré-execução";
                case AtividadeFase.FasePos: return "Pós-execução";
                default: return "Execução";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class Atividade
    {
        public Atividade(Objeto objeto)
        {
            Fase = AtividadeFase.FaseNormal;

            ObjetoRelacionado = objeto;
        }

        public AtividadeFase Fase { get; set; }

        public string Nome { get => ObjetoRelacionado.Nome; }

        public Objeto ObjetoRelacionado { get; private set; }
    }
}
