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

        private readonly ObservableCollection<Objeto> _atividades;

        private readonly ObservableCollection<Tarefa> _tarefas;

        private readonly ObservableCollection<Processo> _processos;

        private XElement _xmlFilhos;

        public ObservableCollection<Objeto> Atividades
        {
            get
            {
                return _atividades;
            }
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

        public Processo(string nome, ObservableCollection<Tarefa> tarefas, ObservableCollection<Processo> processos)
        {
            nomeElementoXml = "processo";
            _nome = nome;
            _atividades = new ObservableCollection<Objeto>();
            _atividades.CollectionChanged += Atividades_CollectionChanged;
            _tarefas = tarefas;
            _processos = processos;
        }

        public Processo(XElement xml, ObservableCollection<Tarefa> tarefas, ObservableCollection<Processo> processos)
        {
            nomeElementoXml = "processo";
            _atividades = new ObservableCollection<Objeto>();
            _atividades.CollectionChanged += Atividades_CollectionChanged;
            _tarefas = tarefas;
            _processos = processos;

            analisarXml(xml);
        }

        protected override void analisarXml(XElement xml)
        {
            string[] elementosnecessarios = { "nome" };

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, PROCESSO_INVALIDO);
            _nome = xml.Element("nome").Value;
            if (xml.Elements("descricao").Count() > 0)
                _descricao = xml.Element("descricao").Value;
            if (xml.Elements("atividades").Count() > 0)
            {
                _xmlFilhos = xml.Element("atividades"); ;
            }
        }

        public void gerarAtividades()
        {
            if (_xmlFilhos == null)
                return;

            foreach (XElement el in _xmlFilhos.Elements())
            {
                if (el.Name == "atividade" && el.HasElements)
                {
                    XElement subel = el.Elements().First();
                    if (subel.Name == "tarefa")
                    {
                        var consultatarefa = from tarefa in _tarefas
                                             where tarefa.Nome == subel.Value
                                             select tarefa;

                        _atividades.Add(consultatarefa.First());
                    }
                    else if (subel.Name == "subprocesso")
                    {
                        var consultaprocesso = from processo in _processos
                                               where processo.Nome == subel.Value
                                               select processo;

                        _atividades.Add(consultaprocesso.First());
                    }
                }
            }
        }

        private void Atividades_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs Args)
        {
            OnPropertyChanged("Atividades");
        }

        public override XElement gerarXml()
        {
            XElement processo;
            XElement atividades;
            XElement atividade;
            string tipoatividade = "";

            processo = base.gerarXml();
            processo.Add(new XElement("nome", Nome));
            processo.Add(new XElement("descricao", Descricao));

            atividades = new XElement("atividades");
            foreach (Objeto objeto in _atividades)
            {
                if (objeto is Tarefa)
                    tipoatividade = "tarefa";
                else if (objeto is Processo)
                    tipoatividade = "subprocesso";
                atividade = new XElement("atividade");
                atividade.Add(new XElement(tipoatividade, objeto.Nome));

                atividades.Add(atividade);
            }

            processo.Add(atividades);

            return processo;
        }

        public override string[] obterEntradas()
        {
            List<string> lista = new List<string>();

            foreach (Objeto o in _atividades)
            {
                if (o.obterEntradas() != null)
                    lista.AddRange(o.obterEntradas());
            }

            return lista.ToArray();
        }
    }
}
