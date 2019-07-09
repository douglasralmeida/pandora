using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Base
{
    public class Processo : Objeto
    {
        private const string PROCESSO_INVALIDO = "O pacote informado possui dados de processos inválidos.";

        private string _descricao;

        private readonly ObservableCollection<Tarefa> _tarefas;

        private readonly ObservableCollection<Processo> _processos;

        private Dictionary<string, List<XElement>> xmlAtividades;

        public ObservableCollection<Atividade> Atividades { get; private set; }

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
            nomeElementoXml = "processo";
            Atividades = new ObservableCollection<Atividade>();
            Atividades.CollectionChanged += Atividades_CollectionChanged;
            _nome = nome;
            _tarefas = tarefas;
            _processos = processos;

            xmlAtividades = new Dictionary<string, List<XElement>>();
        }

        public Processo(XElement xml, ObservableCollection<Tarefa> tarefas, ObservableCollection<Processo> processos)
        {
            nomeElementoXml = "processo";
            Atividades = new ObservableCollection<Atividade>();
            Atividades.CollectionChanged += Atividades_CollectionChanged;
            _tarefas = tarefas;
            _processos = processos;

            xmlAtividades = new Dictionary<string, List<XElement>>();
            analisarXml(xml);
        }

        protected override void analisarXml(XElement xml)
        {
            string[] elementosnecessarios = { "nome" };
            string fase;
            List<XElement> lista;

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, PROCESSO_INVALIDO);
            _nome = xml.Element("nome").Value;
            if (xml.Elements("descricao").Count() > 0)
                _descricao = xml.Element("descricao").Value;
            foreach (XElement el in xml.Elements())
            {
                if (el.Name != "atividades")
                    continue;

                if (el.Attributes("fase").Count() > 0)
                    fase = el.Attribute("fase").Value;
                else
                    fase = "normal";
                if (xmlAtividades.ContainsKey(fase))
                    xmlAtividades.TryGetValue(fase, out lista);
                else
                {
                    lista = new List<XElement>();
                    xmlAtividades.Add(fase, lista);
                }
                lista.AddRange(el.Elements("atividade"));
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
                if (el.Name == "atividade" && el.HasElements)
                {
                    XElement subel = el.Elements().First();
                    if (subel.Name == "tarefa")
                    {
                        var consultatarefa = from tarefa in _tarefas
                                             where tarefa.Nome == subel.Value
                                             select tarefa;

                        atividade = new Atividade(consultatarefa.First());
                        atividade.Fase = fase;
                        Atividades.Add(atividade);
                    }
                    else if (subel.Name == "subprocesso")
                    {
                        var consultaprocesso = from processo in _processos
                                               where processo.Nome == subel.Value
                                               select processo;

                        atividade = new Atividade(consultaprocesso.First());
                        atividade.Fase = fase;
                        Atividades.Add(atividade);
                    }
                }
            }
        }

        public override XElement gerarXml()
        {
            string[] xmlFases = { "normal", "pre", "pos" };
            string tipoatividade = "";
            XElement atividade;
            XElement atividades;
            XElement processo;

            processo = base.gerarXml();
            processo.Add(new XElement("nome", Nome));
            processo.Add(new XElement("descricao", Descricao));
            atividades = new XElement("atividades");
            atividades.SetAttributeValue("fase", xmlFases[(int)Fase]);
            foreach (Objeto objeto in Atividades)
            {
                if (objeto is Tarefa)
                    tipoatividade = "tarefa";
                else if (objeto is Processo)
                    tipoatividade = "subprocesso";
                atividade = new XElement("atividade");
                atividade.Add(new XElement(tipoatividade, objeto.Nome));

                atividades.Add(atividade);
            }

            return processo;
        }

        public override string[] obterEntradas()
        {
            List<string> lista = new List<string>();

            foreach (Objeto o in Atividades)
            {
                if (o.obterEntradas() != null)
                    lista.AddRange(o.obterEntradas());
            }

            return lista.ToArray();
        }
    }
}
