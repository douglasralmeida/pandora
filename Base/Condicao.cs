

namespace Base
{
    public class Condicao : Objeto
    {
        private const string CONDICAO_INVALIDA = "O pacote informado possui dados de condições inválidos.";

        private Modulo _modulo;

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

        public ObservableCollection<Partao> Portoes { get; private set; }

        public Condicao(string nome)
        {
            prepararCondicao();
        }

        public Tarefa(XElement xml)
        {
            prepararCondicao();
            analisarXml(xml);
        }

        protected override void analisarXml(XElement xml)
        {
            int i;
            string nomemodulo;
            Portao novoportao;
            XElement portoes;
            string[] elementosnecessarios = { "nome", "descricao", "modulo" };

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, TAREFA_INVALIDA);
            _nome = xml.Element("nome").Value;
            _descricao = xml.Element("descricao").Value;
            nomemodulo = xml.Element("modulo").Value;
            _modulo = BibliotecaPadrao.Biblioteca.Obter(nomemodulo);

            if (xml.Elements("portoes").Count() > 0)
            {
                portoes = xml.Element("portoes");
                i = 1;
                foreach (XElement el in portoes.Elements())
                {
                    if (el.Name == "portao")
                    {
                        novoportao = carregarPortao(el, i);
                        if (novoportao != null)
                        {
                            Portoes.Add(novoportao);
                            i++;
                        }
                    }
                }
            }
        }

        private Portao carregarPortao(XElement xml, int i)
        {
            Portao portao;

            portao = new Portao(xml, i);

            return portao;
        }

        public override XElement gerarXml()
        {
            XElement condicao;
            XElement portoes;

            condicao = base.gerarXml();
            condicao.Add(new XElement("nome", Nome));
            condicao.Add(new XElement("descricao", Descricao));
            condicao.Add(new XElement("modulo", Modulo));
            portoes = new XElement("portoes");
            foreach (Portao portao in Portoes)
            {
                portoes.Add(portao.gerarXml());
            }
            condicao.Add(portoes);

            return condicao;
        }

        public int getPortoesCount()
        {
            return Portoes.Count;
        }

        void Portoes_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs Args)
        {
            OnPropertyChanged("Portoes");
        }

        private void prepararCondicao()
        {
            nomeElementoXml = "condicao";
            Portoes = new ObservableCollection<Portao>();
            Portoes.CollectionChanged += Portoes_CollectionChanged;
        }

        public override string ToString()
        {
            return _nome;
        }
    }
}