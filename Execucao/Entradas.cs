using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Execucao
{
    public class EntradaVariavel
    {
        public string Nome { get; private set; }

        public string Valor { get; set; }

        public EntradaVariavel()
        {
            Nome = "";
            Valor = "";
        }

        public EntradaVariavel(string nome, string valor)
        { 
            Nome = nome;
            Valor = valor;
        }
    }

    public class Entrada
    {
        public ObservableCollection<EntradaVariavel> Variaveis { get; set; }
        public bool TudoVazio
        {
            get
            {
                bool resultado = true;

                foreach(EntradaVariavel ev in Variaveis)
                    resultado &= (ev.Valor.Length == 0);

                return resultado;
            }
        }

        public Entrada()
        {
            Variaveis = new ObservableCollection<EntradaVariavel>();
        }

        public void AdicionarVariavel(string nome, string valor)
        {
            Variaveis.Add(new EntradaVariavel(nome, valor));
        }

        public string GetValor(string nome)
        {
            foreach(EntradaVariavel ev in Variaveis)
            {
                if (ev.Nome == nome)
                    return ev.Valor;
            }

            return null;
        }
    }

    class Entradas
    {
        public ObservableCollection<Entrada> Lista { get; private set; }

        public Entradas()
        {
            Lista = new ObservableCollection<Entrada>();
        }

        public void Adicionar(Entrada e)
        {
            Lista.Add(e);
        }

        public void Limpar()
        {
            Lista.Clear();
        }

        public string[,] Obter(string[] cabecalho)
        {
            int i = 0;
            int j;
            string[,] matriz = new string[Lista.Count, cabecalho.Length];

            foreach (Entrada entrada in Lista)
            {
                j = 0;
                foreach (EntradaVariavel v in entrada.Variaveis)
                {
                    if (v.Nome == cabecalho[j])
                        matriz[i, j] = v.Valor;
                    else
                        matriz[i, j] = "";
                    j++;
                }
                i++;
            }

            return matriz;
        }

        public int Quantidade()
        {
            return Lista.Count();
        }
    }
}
