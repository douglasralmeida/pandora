using System;
using System.ComponentModel;
using System.Globalization;
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

    public class Atividade : IEquatable<Atividade>
    {
        public Atividade(Objeto objeto)
        {
            Fase = AtividadeFase.FaseNormal;

            ObjetoRelacionado = objeto;
        }

        public AtividadeFase Fase { get; set; }

        public string Nome => ObjetoRelacionado != null ? ObjetoRelacionado.Nome : "";

        public Objeto ObjetoRelacionado { get; private set; }

        public bool Equals(Atividade outra)
        {
            return null != outra && ObjetoRelacionado == outra.ObjetoRelacionado;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Atividade);
        }

        public override int GetHashCode()
        {
            string nomecompleto;
            if (ObjetoRelacionado is Tarefa)
                nomecompleto = "tarefa " + ObjetoRelacionado.Nome;
            else if (ObjetoRelacionado is Processo)
                nomecompleto = "processo " + ObjetoRelacionado.Nome;
            else
                nomecompleto = "";

            return nomecompleto.GetHashCode();
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
