using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private string descricao;

        private List<string> entradas;

        private int proximaEntrada;

        private int etapa;

        private string nome;

        private ObservableCollection<Operacao> listaOperacoes;

        public string Descricao
        {
            get { return this.descricao; }
            set { this.descricao = value; }
        }

        public string Nome
        {
            get { return this.nome; }
            set { this.nome = value; }
        }

        public ObservableCollection<Operacao> Operacoes => this.listaOperacoes;

        public Tarefa(string nome)
        {
            this.nome = nome;
            this.entradas = new List<string>();
            this.listaOperacoes = new ObservableCollection<Operacao>();
            this.etapa = 0;
        }

        public Tarefa(XElement xml)
        {
            this.entradas = new List<string>();
            this.listaOperacoes = new ObservableCollection<Operacao>();
            this.etapa = 0;

            analisarXml(xml);
        }

        public void adicionarEntrada(string entrada)
        {
            this.entradas.Add(entrada);
        }

        public void adicionarEntradas(string[] entradas)
        {
            foreach (string e in entradas)
            {
                this.entradas.Add(e);
            }
        }
        protected override void analisarXml(XElement xml)
        {
            int i;
            Operacao novaoperacao;
            XElement operacoes;
            string[] elementosnecessarios = { "nome", "descricao" };

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, TAREFA_INVALIDA);
            this.nome = xml.Element("nome").Value;
            this.descricao = xml.Element("descricao").Value;

            if (xml.Elements("operacoes").Count() > 0)
            {
                operacoes = xml.Element("operacoes");
                i = 0;
                foreach (XElement el in operacoes.Elements())
                {
                    if (el.Name == "operacao")
                    {
                        novaoperacao = carregarOperacao(el, i);
                        if (novaoperacao != null)
                        {
                            listaOperacoes.Add(novaoperacao);
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

        public string getDescricao()
        {
            return this.descricao;
        }

        public int getOperacoesCount()
        {
            return this.listaOperacoes.Count;
        }

        public void iniciar()
        {
            etapa = 0;
            proximaEntrada = 0;
        }

        public string proximaOperacao()
        {
            int i;
            //Operacao operacao;

            StringBuilder builder = new StringBuilder(this.listaOperacoes.ElementAt(etapa).getComando());

            while (builder.ToString().IndexOf(ENTRADA) != -1)
            {
                i = builder.ToString().IndexOf(ENTRADA);
                builder.Remove(i, ENTRADA.Length);
                builder.Insert(i, entradas[proximaEntrada]);
                proximaEntrada++;
            }
            etapa++;

            return builder.ToString();
        }

        public bool possuiProximaOperacao()
        {
            return (etapa < this.getOperacoesCount());
        }

        public override string ToString()
        {
            return this.nome;
        }
    }
}
