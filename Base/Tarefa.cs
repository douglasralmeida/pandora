using Execucao;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Base
{
    public class Tarefa : Objeto
    {
        private const string TAREFA_INVALIDA = "O pacote informado possui dados de tarefas inválidos.";

        private const string ENTRADA = "{ENTRADA}";

        private string _descricao;

        private List<string> _entradas;

        private Modulo _modulo;

        private int _proximaEntrada;

        private int _etapa;

        public string Descricao
        {
            get =>_descricao;

            set
            {
                if (_descricao != value)
                {
                    _descricao = value;
                    OnPropertyChanged("Descricao");
                }
            }
        }

        public Modulo Modulo
        {
            get => _modulo;

            set
            {
                if (_modulo != value)
                {
                    _modulo = value;
                    OnPropertyChanged("Modulo");
                }
            }
        }

        public ObservableCollection<Operacao> Operacoes { get; private set; }

        public Tarefa(string nome)
        {
            nomeElementoXml = "tarefa";
            prepararTarefa();
        }

        public Tarefa(XElement xml)
        {
            prepararTarefa();
            analisarXml(xml);
        }

        public void adicionarEntrada(string entrada)
        {
            _entradas.Add(entrada);
        }

        public void adicionarEntradas(string[] entradas)
        {
            foreach (string e in entradas)
            {
                _entradas.Add(e);
            }
        }

        protected override void analisarXml(XElement xml)
        {
            int i;
            string nomemodulo;
            Operacao novaoperacao;
            XElement operacoes;
            string[] elementosnecessarios = { "nome", "descricao", "modulo" };

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, TAREFA_INVALIDA);
            _nome = xml.Element("nome").Value;
            _descricao = xml.Element("descricao").Value;
            nomemodulo = xml.Element("modulo").Value;
            _modulo = BibliotecaPadrao.Biblioteca.Obter(nomemodulo);

            if (xml.Elements("operacoes").Count() > 0)
            {
                operacoes = xml.Element("operacoes");
                i = 1;
                foreach (XElement el in operacoes.Elements())
                {
                    if (el.Name == "operacao")
                    {
                        novaoperacao = carregarOperacao(el, i);
                        if (novaoperacao != null)
                        {
                            Operacoes.Add(novaoperacao);
                            i++;
                        }
                    }
                }
            }
        }

        private Operacao carregarOperacao(XElement xml, int i)
        {
            Operacao operacao;

            operacao = new Operacao(xml, i);

            return operacao;
        }

        public override XElement gerarXml()
        {
            XElement tarefa;
            XElement operacoes;

            tarefa = base.gerarXml();
            tarefa.Add(new XElement("nome", Nome));
            tarefa.Add(new XElement("descricao", Descricao));
            tarefa.Add(new XElement("modulo", Modulo));
            operacoes = new XElement("operacoes");
            foreach (Base.Operacao operacao in Operacoes)
            {
                operacoes.Add(operacao.gerarXml());
            }

            tarefa.Add(operacoes);

            return tarefa;
        }

        public int getOperacoesCount()
        {
            return Operacoes.Count;
        }

        public void iniciar()
        {
            _etapa = 0;
            _proximaEntrada = 0;
        }

        public override string[] obterEntradas()
        {
            List<string> lista = new List<string>();

            foreach(Operacao o in Operacoes)
            {
                if (o.obterEntradas() != null)
                    lista.AddRange(o.obterEntradas());
            }

            return lista.ToArray();
        }

        void Operacoes_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs Args)
        {
            OnPropertyChanged("Operacoes");
        }

        public string proximaOperacao()
        {
            int i;
            StringBuilder builder = new StringBuilder(Operacoes.ElementAt(_etapa).Nome);

            while (builder.ToString().IndexOf(ENTRADA) != -1)
            {
                i = builder.ToString().IndexOf(ENTRADA);
                builder.Remove(i, ENTRADA.Length);
                builder.Insert(i, _entradas[_proximaEntrada]);
                _proximaEntrada++;
            }
            _etapa++;

            return builder.ToString();
        }

        public bool possuiProximaOperacao()
        {
            return (_etapa < this.getOperacoesCount());
        }

        private void prepararTarefa()
        {
            nomeElementoXml = "tarefa";
            _entradas = new List<string>();
            Operacoes = new ObservableCollection<Operacao>();
            Operacoes.CollectionChanged += Operacoes_CollectionChanged;
            _etapa = 0;
        }

        public void reprocessarOperacaoIds()
        {
            int i = 1;

            foreach (Base.Operacao operacao in Operacoes)
            {
                operacao.Id = i;
                i++;
            }
        }

        public override string ToString()
        {
            return _nome;
        }
    }
}
