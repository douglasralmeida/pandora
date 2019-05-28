using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Base
{
    //
    // Resumo:
    //     Representa o método que manipulará o evento Base.Pacote.ProcessoAdded
    //     gerado quando um pacote é adicionado no pacote.
    //
    // Parâmetros:
    //   sender:
    //     A fonte do evento.
    //
    //   processo:
    //     O processo adicionado que gerou o evento.
    public delegate void ProcessoAddedEventHandler(object sender, Processo processo);

    //
    // Resumo:
    //     Representa o método que manipulará o evento Base.Pacote.TarefaAdded
    //     gerado quando uma tarefa é adicionada no pacote.
    //
    // Parâmetros:
    //   sender:
    //     A fonte do evento.
    //
    //   tarefa:
    //     A tarefa adicionada que gerou o evento.
    public delegate void TarefaAddedEventHandler(object sender, Tarefa tarefa);

    class Pacote : Objeto
    {
        private const string PAC_INVALIDO = "O arquivo informado não é um pacote de processos do Pandora válido.";

        private const string PAC_SEMCABECA = "O pacote de processos do Pandora informado não possui um cabeçalho válido.";

        private const string PAC_SEMCONTEUDO = "O pacote de processos do Pandora informado não possui um conteúdo válido.";

        private const string PAC_SEMTAREFAS = "O pacote de processos do Pandora informado não possui uma lista de tarefas válida.";

        private const string PAC_SEMPRCOS = "O pacote de processos do Pandora informado não possui uma lista de processos válida.";

        private const string XMLVER = "1";

        private string _nomegerador;

        private readonly ObservableCollection<Tarefa> _tarefas;

        private readonly ObservableCollection<Processo> _processos;

        public event ProcessoAddedEventHandler ProcessoAdded;

        public event TarefaAddedEventHandler TarefaAdded;

        public ObservableCollection<Processo> Processos
        {
            get
            {
                return _processos;
            }
        }

        public ObservableCollection<Tarefa> Tarefas
        {
            get
            {
                return _tarefas;
            }
        }

        public Pacote(string nomecriador)
        {
            this._nomegerador = nomecriador;
            this.nomeElementoXml = "pacote";
            _tarefas = new ObservableCollection<Tarefa>();
            _tarefas.CollectionChanged += Tarefas_CollectionChanged;
            _processos = new ObservableCollection<Processo>();
            _processos.CollectionChanged += Processos_CollectionChanged;
        }

        public Pacote(XElement xml)
        {
            this.nomeElementoXml = "pacote";
            _tarefas = new ObservableCollection<Tarefa>();
            _tarefas.CollectionChanged += Tarefas_CollectionChanged;
            _processos = new ObservableCollection<Processo>();
            _processos.CollectionChanged += Processos_CollectionChanged;
            analisarXml(xml);
        }

        protected override void analisarXml(XElement xml)
        {
            XElement pacote, cabecalho, conteudo;

            XMLAuxiliar.checarNomeXml(xml, nomeElementoXml, PAC_INVALIDO);
            pacote = xml;
            if (!pacote.HasElements)
                throw new PandoraException(PAC_SEMCABECA);

            cabecalho = pacote.Elements().First();
            XMLAuxiliar.checarNomeXml(cabecalho, "cabecalho", PAC_SEMCABECA);
            carregarCabecalho(cabecalho);

            XMLAuxiliar.checarFilhoXML(pacote, "conteudo", PAC_SEMCONTEUDO);
            conteudo = pacote.Element("conteudo");
            carregarConteudo(conteudo);
        }

        private void carregarCabecalho(XElement cabecalho)
        {
            XElement el;

            XMLAuxiliar.checarFilhoXML(cabecalho, "versao", PAC_SEMCABECA);
            if (cabecalho.Element("versao").Value != "1")
                throw new PandoraException(PAC_SEMCABECA);

            XMLAuxiliar.checarFilhoXML(cabecalho, "geracao", PAC_SEMCABECA);
            el = cabecalho.Element("geracao");

            if (el.Attributes("nome").Count() > 0)
                _nomegerador = el.Attribute("nome").Value;
        }

        private void carregarConteudo(XElement conteudo)
        {
            XElement tarefas, processos;
            Processo novoprocesso;
            Tarefa novatarefa;

            XMLAuxiliar.checarFilhoXML(conteudo, "tarefas", PAC_SEMCONTEUDO);
            tarefas = conteudo.Element("tarefas");

            XMLAuxiliar.checarFilhoXML(conteudo, "processos", PAC_SEMCONTEUDO);
            processos = conteudo.Element("processos");

            foreach (XElement el in tarefas.Elements())
            {
                if (el.Name == "tarefa")
                {
                    novatarefa = carregarTarefa(el);
                    if (novatarefa != null)
                        _tarefas.Add(novatarefa);
                }
            }
            foreach (XElement el in processos.Elements())
            {
                if (el.Name == "processo")
                {
                    novoprocesso = carregarProcesso(el);
                    if (novoprocesso != null)
                        _processos.Add(novoprocesso);
                }
            }
        }

        private Processo carregarProcesso(XElement xml)
        {
            Processo processo;

            processo = new Processo(xml, _tarefas, _processos);

            return processo;
        }

        private Tarefa carregarTarefa(XElement xml)
        {
            Tarefa tarefa;

            tarefa = new Tarefa(xml);

            return tarefa;
        }

        public void excluirTarefa(Tarefa tarefa)
        {
            Tarefas.Remove(tarefa);
        }

        public override XElement gerarXml()
        {
            List<XAttribute> builder = new List<XAttribute>();
            XElement cabecalho, geracao, conteudo, pacote;
            XElement processos, tarefas;

            builder.Add(new XAttribute("nome", _nomegerador));
            builder.Add(new XAttribute("data", DateTime.Now));

            geracao = new XElement("geracao");
            geracao.Add(builder.ToArray());

            cabecalho = new XElement("cabecalho");
            cabecalho.Add(new XElement("versao", XMLVER));
            cabecalho.Add(geracao);

            processos = new XElement("processos");
            //adidionar cada processo aqui

            tarefas = new XElement("tarefas");
            foreach (Base.Tarefa _tarefa in _tarefas)
            {
                tarefas.Add(_tarefa.gerarXml());
            }

            conteudo = new XElement("conteudo");
            conteudo.Add(processos);
            conteudo.Add(tarefas);

            pacote = new XElement(nomeElementoXml);
            pacote.Add(cabecalho);
            pacote.Add(conteudo);

            return pacote;
        }

        public void inserirProcesso()
        {
            Processo novoprocesso;

            novoprocesso = new Processo("NovoProcesso", _tarefas, _processos);
            _processos.Add(novoprocesso);

            OnProcessoAdded(novoprocesso);
        }

        public void inserirTarefa()
        {
            Tarefa novatarefa;

            novatarefa = new Tarefa("NovaTarefa");
            _tarefas.Add(novatarefa);

            OnTarefaAdded(novatarefa);
        }

        protected void OnProcessoAdded(Processo processo)
        {
            ProcessoAdded?.Invoke(this, processo);
        }

        protected void OnTarefaAdded(Tarefa tarefa)
        {
            TarefaAdded?.Invoke(this, tarefa);
        }

        void Processos_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs Args)
        {
            OnPropertyChanged("Processos");
        }

        void Tarefas_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs Args)
        {
            OnPropertyChanged("Tarefas");
        }
    }
}
