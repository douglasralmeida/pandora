using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Xml.Linq;

namespace Base
{
    public class Processo : Objeto
    {
        private const string PROCESSO_INVALIDO = "O pacote informado possui dados de processos inválidos.";

        private string _descricao;

        private ObservableCollection<Tarefa> _tarefas;

        private ObservableCollection<Processo> _processos;

        private Dictionary<string, List<XElement>> xmlAtividades;

        public ObservableCollection<Atividade> Atividades { get; private set; }

        public ICollectionView Visao => CollectionViewSource.GetDefaultView(Atividades);

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

        public Processo(string nome, ObservableCollection<Tarefa> tarefas, ObservableCollection<Processo> processos)
        {
            _nome = nome;
            prepararProcesso(tarefas, processos);
        }

        public Processo(XElement xml, ObservableCollection<Tarefa> tarefas, ObservableCollection<Processo> processos)
        {
            prepararProcesso(tarefas, processos);
            analisarXml(xml);
        }

        private void prepararProcesso(ObservableCollection<Tarefa> tarefas, ObservableCollection<Processo> processos)
        {
            nomeElementoXml = "processo";
            _tarefas = tarefas;
            _processos = processos;
            xmlAtividades = new Dictionary<string, List<XElement>>();
            Atividades = new ObservableCollection<Atividade>();
            Atividades.CollectionChanged += Atividades_CollectionChanged;
            Visao.GroupDescriptions.Add(new PropertyGroupDescription("Fase", new AtividadeFaseConverter()));
        }

        public void adicionarAtividade(Atividade atividade)
        {
            Atividades.Add(atividade);
            Visao.Refresh();
        }

        protected override void analisarXml(XElement xml)
        {
            string[] elementosnecessarios = { "nome" };
            string fase;
            List<XElement> lista;

            //checa se o nó XML contém os elementos obrigatórios e os carrega
            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, PROCESSO_INVALIDO);
            _nome = xml.Element("nome").Value;
            if (xml.Elements("descricao").Count() > 0)
                _descricao = xml.Element("descricao").Value;

            //verifica o nó filho 'atividades'
            foreach (XElement el in xml.Elements())
            {
                if (el.Name != "atividades")
                    continue;
                
                //varre todos os filhos do nó 'atividades'
                foreach (XElement ativ in el.Elements())
                {
                    if (ativ.Name != "atividade")
                    continue;
                    fase = "normal"; //se não tiver fase informada, será normal

                    //carrega a fase da atividade
                    if (ativ.Attributes("fase").Count() > 0)
                        fase = ativ.Attribute("fase").Value;

                    //carrega a tarefa ou o subproceso correspondente a atividade numa lista apropriada
                    if (xmlAtividades.ContainsKey(fase))
                        xmlAtividades.TryGetValue(fase, out lista);
                    else
                    {
                        lista = new List<XElement>();
                        xmlAtividades.Add(fase, lista);
                    }
                    lista.Add(ativ.Elements().First());
                }
            }
        }

        private void Atividades_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs Args)
        {
            OnPropertyChanged("Atividades");
        }

        //carrega as atividades de um processo
        public void gerarAtividades()
        {
            List<XElement> lista;

            xmlAtividades.TryGetValue("normal", out lista);
            if (lista != null)
                gerarListaAtividades(AtividadeFase.FaseNormal, lista);
            xmlAtividades.TryGetValue("pre", out lista);
            if (lista != null)
                gerarListaAtividades(AtividadeFase.FasePre, lista);
            xmlAtividades.TryGetValue("pos", out lista);
            if (lista != null)
                gerarListaAtividades(AtividadeFase.FasePos, lista);
        }

        private void gerarListaAtividades(AtividadeFase fase, List<XElement> lista)
        {
            Atividade atividade;

            foreach (XElement el in lista)
            {
                if (el.Name == "tarefa")
                {
                    var consultatarefa = from tarefa in _tarefas
                                         where tarefa.Nome == el.Value
                                         select tarefa;

                    atividade = new Atividade(consultatarefa.First());
                    atividade.Fase = fase;
                    Atividades.Add(atividade);
                }
                else if (el.Name == "subprocesso")
                {
                    var consultaprocesso = from processo in _processos
                                           where processo.Nome == el.Value
                                           select processo;

                    atividade = new Atividade(consultaprocesso.First());
                    atividade.Fase = fase;
                    Atividades.Add(atividade);
                }
            }
        }

        public override XElement gerarXml()
        {
            string[] xmlFases = { "normal", "pre", "pos" };
            string tipoatividade = "";
            XElement xatividade;
            XElement xatividades;
            XElement xprocesso;
            XElement xlistapre;
            XElement xlistanormal;
            XElement xlistapos;

            XElement xlista;

            xprocesso = base.gerarXml();
            xprocesso.Add(new XElement("nome", Nome));
            xprocesso.Add(new XElement("descricao", Descricao));
            xatividades = new XElement("atividades");
            xlistapre = new XElement("atividades");
            xlistanormal = new XElement("atividades");
            xlistapos = new XElement("atividades");
            foreach (Atividade atividade in Atividades)
            {
                if (atividade.ObjetoRelacionado is Tarefa)
                    tipoatividade = "tarefa";
                else if (atividade.ObjetoRelacionado is Processo)
                    tipoatividade = "subprocesso";
                xatividade = new XElement("atividade");
                switch (atividade.Fase)
                {
                    case AtividadeFase.FasePre:
                        xlista = xlistapre;
                        break;
                    case AtividadeFase.FasePos:
                        xlista = xlistapos;
                        break;
                    default:
                        xlista = xlistanormal;
                        break;
                }
                xatividade.Add(new XElement(tipoatividade, atividade.ObjetoRelacionado.Nome));
                xatividade.SetAttributeValue("fase", xmlFases[(int)atividade.Fase]);
                xlista.Add(xatividade);
            }
            xatividades.Add(xlistapre.Elements());
            xatividades.Add(xlistanormal.Elements());
            xatividades.Add(xlistapos.Elements());
            xprocesso.Add(xatividades);

            return xprocesso;
        }

        public override string[] obterEntradas()
        {
            List<string> lista = new List<string>();

            foreach (Atividade a in Atividades)
            {
                if (a.ObjetoRelacionado.obterEntradas() != null)
                    lista.AddRange(a.ObjetoRelacionado.obterEntradas());
            }

            return lista.ToArray();
        }
    }
}