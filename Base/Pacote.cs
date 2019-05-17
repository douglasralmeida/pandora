using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Base
{
    class Pacote : Objeto
    {
        private const string XMLVER = 1;

        private string nomegerador;

        public Pacote(string nomecriador)
        {
            this.nomegerador = nomecriador;
            this.nomeElementoXml = "pacote";
        }

        public override bool analisarXml(XElement xml)
        {

        }
        public override XElement gerarXml()
        {
            List<XAttribute> builder = new List<XAttribute>();
            XElement cabecalho, geracao, conteudo, pacote;
            XElement processos, tarefas;

            builder.Add(new XAttribute("nome", nomegerador));
            builder.Add(new XAttribute("data", DateTime.Now);

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
