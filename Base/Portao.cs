

using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace Base
{
    public enum OperadorComparacao
    {
        MenorIgual,
        Menor,
        MaiorIgual,
        Maior,
        Igual,
        Diferente
    };

    public enum OperadorLogico
    {
        Nenhum,
        E,
        Ou,
        OuExclusivo
    }

    public enum ComparacaoTipo
    {
        ComparacaoVariavel,
        ComparacaoOperacao
    }

    public class Comparacao
    {
        string nome;

        ComparacaoTipo tipo;

        OperadorComparacao operadorComparacao;

        string valor;

        OperadorLogico operadorLogico;

        Operacao operacao;
    }

    public class Portao : Objeto
    {
        private const string PORTAO_INVALIDO = "O pacote informado possui dados de portões inválidos.";

        private int _id;

        public ObservableCollection<Comparacao> Comparacoes;

        public Objeto Destino;
        private int i;

        public Portao(XElement xml, int i) : base(xml)
        {
            this.i = i;
        }
    }
}