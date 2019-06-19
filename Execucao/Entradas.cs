using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Entrada()
        {
            Variaveis = new ObservableCollection<EntradaVariavel>();
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
    }
}
