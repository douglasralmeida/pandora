using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    class Tarefa
    {
        private string descricao;

        private List<string> entradas;

        private int proximaEntrada;

        private int etapa;

        private string nome;       

        private List<Operacao> operacoes;

        public Tarefa(string nome)
        {
            this.nome = nome;
            this.entradas = new List<string>();
            this.operacoes = new List<Operacao>();
            this.etapa = 0;
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

        public string getDescricao()
        {
            return this.descricao;
        }

        public string getNome()
        {
            return this.nome;
        }

        public int getOperacoesCount()
        {
            return this.operacoes.Count;
        }

        public void iniciar()
        {
            etapa = 0;
            proximaEntrada = 0;
        }

        public string proximaOperacao()
        {
            int i;
            Operacao operacao;

            StringBuilder builder = new StringBuilder(this.operacoes(etapa).getComando());

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

        public void setOperacao(string novocomando)
        {
            this.comando = novocomando;
        }

        public override string ToString()
        {
            return this.nome;
        }
    }
}
