using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Base
{
    public class Tarefa : Objeto
    {
        private const string TAREFA_INVALIDA = "O pacote informado possui dados de tarefas inválidos.";

        private const string ENTRADA = "{ENTRADA}";

        private string _descricao;

        private List<string> _entradas;

        private int _proximaEntrada;

        private int _etapa;

        private string _nome;

        private ObservableCollection<Operacao> _operacoes;

        public Tarefa(string nome)
        {
            _nome = nome;
            _entradas = new List<string>();
            _operacoes = new ObservableCollection<Operacao>();
            _operacoes.CollectionChanged += Operacoes_CollectionChanged;
            _etapa = 0;
        }

        public Tarefa(XElement xml)
        {
            _entradas = new List<string>();
            _operacoes = new ObservableCollection<Operacao>();
            _operacoes.CollectionChanged += Operacoes_CollectionChanged;
            _etapa = 0;

            analisarXml(xml);
        }

        public string Descricao
        {
            get
            {
                return _descricao;
            }
            set
            {
                if (_descricao != value)
                {
                    _descricao = value;
                    OnPropertyChanged("Descricao");
                }
            }
        }

        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                if (_nome != value)
                {
                    _nome = value;
                    OnPropertyChanged("Nome");
                }
            }
        }

        public ObservableCollection<Operacao> Operacoes
        {
            get
            {
                return _operacoes;
            }
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
            Operacao novaoperacao;
            XElement operacoes;
            string[] elementosnecessarios = { "nome", "descricao" };

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, TAREFA_INVALIDA);
            _nome = xml.Element("nome").Value;
            _descricao = xml.Element("descricao").Value;

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
                            _operacoes.Add(novaoperacao);
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

        public int getOperacoesCount()
        {
            return _operacoes.Count;
        }

        public void iniciar()
        {
            _etapa = 0;
            _proximaEntrada = 0;
        }

        void Operacoes_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs Args)
        {
            OnPropertyChanged("Operacoes");
        }

        public string proximaOperacao()
        {
            int i;
            StringBuilder builder = new StringBuilder(_operacoes.ElementAt(_etapa).Comando);

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
