using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Base
{
    class Pacote : Objeto
    {
        private const string PAC_INVALIDO = "O arquivo informado não é um pacote de processos do Pandora válido.";

        private const string PAC_SEMCABECA = "O pacote de processos do Pandora informado não possui um cabeçalho válido.";

        private const string PAC_SEMCONTEUDO = "O pacote de processos do Pandora informado não possui um conteúdo válido.";

        private const string PAC_SEMTAREFAS = "O pacote de processos do Pandora informado não possui uma lista de tarefas válida.";

        private const string PAC_SEMPRCOS = "O pacote de processos do Pandora informado não possui uma lista de processos válida.";

        private const string XMLVER = "1";

        private string nomegerador;

        private List<Tarefa> listaTarefas;

        private List<Processo> listaProcessos;

        public Pacote(string nomecriador)
        {
            this.nomegerador = nomecriador;
            this.nomeElementoXml = "pacote";
            listaProcessos = new List<Processo>();
            listaTarefas = new List<Tarefa>();
        }

        public Pacote(XElement xml)
        {
            this.nomeElementoXml = "pacote";
            listaProcessos = new List<Processo>();
            listaTarefas = new List<Tarefa>();
            analisarXml(xml);
        }

        private void carregarCabecalhoXml(XElement cabecalho)
        {
            XElement el;

            XMLAuxiliar.checarFilhoXML(cabecalho, "versao", PAC_SEMCABECA);
            if (cabecalho.Element("versao").Value != "1")
                throw new PandoraException(PAC_SEMCABECA);

            XMLAuxiliar.checarFilhoXML(cabecalho, "geracao", PAC_SEMCABECA);
            el = cabecalho.Element("geracao");

            if (el.Attributes("nome").Count() > 0)
                nomegerador = el.Attribute("nome").Value;
        }

        private void carregarConteudoXML(XElement conteudo)
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
                        listaTarefas.Add(novatarefa);
                }
            }
            foreach (XElement el in processos.Elements())
            {
                if (el.Name == "processo")
                {
                    novoprocesso = carregarProcesso(el);
                    if (novoprocesso != null)
                        listaProcessos.Add(novoprocesso);
                }
            }
        }

        private Processo carregarProcesso(XElement xml)
        {
            Processo processo;

            processo = new Processo(xml);

            return processo;
        }

        private Tarefa carregarTarefa(XElement xml)
        {
            Tarefa tarefa;

            tarefa = new Tarefa(xml);

            return tarefa;
        }

        public List<Tarefa> getListaTarefas()
        {
            return listaTarefas;
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
            carregarCabecalhoXml(cabecalho);

            XMLAuxiliar.checarFilhoXML(pacote, "conteudo", PAC_SEMCONTEUDO);
            conteudo = pacote.Element("conteudo");
            carregarConteudoXML(conteudo);
        }
        public override XElement gerarXml()
        {
            List<XAttribute> builder = new List<XAttribute>();
            XElement cabecalho, geracao, conteudo, pacote;
            XElement processos, tarefas;

            builder.Add(new XAttribute("nome", nomegerador));
            builder.Add(new XAttribute("data", DateTime.Now));

            geracao = new XElement("geracao");
            geracao.Add(builder.ToArray());

            cabecalho = new XElement("cabecalho");
            cabecalho.Add(new XElement("versao", XMLVER));
            cabecalho.Add(geracao);

            processos = new XElement("processos");
            //adidionar cada processo aqui
            tarefas = new XElement("tarefas");
            //adicionar cada tarefa aqui

            conteudo = new XElement("conteudo");
            conteudo.Add(processos);
            conteudo.Add(tarefas);

            pacote = new XElement(nomeElementoXml);
            pacote.Add(cabecalho);
            pacote.Add(conteudo);

            return pacote;
        }
    }
}
