using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Base
{
    class Pacote : Objeto
    {
        private const string XMLVER = "1";

        private string nomegerador;

        public Pacote(string nomecriador)
        {
            this.nomegerador = nomecriador;
            this.nomeElementoXml = "pacote";
        }

        public override void analisarXml(XElement xml)
        {
            const string PAC_INVALIDO = "O arquivo informado não é um pacote de processos do Pandora válido.";
            const string PAC_SEMCABECA = "O pacote de processos do Pandora informado não possui um cabeçalho válido.";
            XElement pacote, cabecalho, conteudo;
            XElement el;
            IEnumerable<XElement> elementos;

            if (xml.Name != nomeElementoXml)
                throw new PandoraException(PAC_INVALIDO);
            pacote = xml;
            if (!pacote.HasElements)
                throw new PandoraException(PAC_SEMCABECA);
            cabecalho = pacote.Elements().First();
            if (cabecalho.Name != "cabecalho")
                throw new PandoraException(PAC_SEMCABECA);
            elementos = cabecalho.Elements("geracao");
            if (elementos.Count() > 0)
                el = elementos.First();
            else
                throw new PandoraException(PAC_SEMCABECA);
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
