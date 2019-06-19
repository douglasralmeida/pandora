using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Execucao
{
    public enum ErroTipo
    {
        Aviso,
        Erro,
        Informacao
    }

    public class Erro
    {
        public ErroTipo Tipo { get; private set; }

        public string Codigo { get; private set; }

        public string Descricao { get; set; }

        public string[] Valores { get; set; }

        public Erro()
        {
            Codigo = "";
            Descricao = "";
            Tipo = ErroTipo.Erro;
            Valores = new string[0];
        }

        public Erro(string codigo, string desc, ErroTipo tipo, string[] valores)
        {
            Codigo = codigo;
            Descricao = desc;
            Tipo = tipo;
            Valores = valores;
        }
    }

    public static class ErroCatalogo
    {
        private static List<Erro> ListaErros = new List<Erro>()
        {
            new Erro("CT0001", "Nenhuma carteira foi aberta.", ErroTipo.Erro, new string[0]),
            new Erro("CT0002", "O item da carteira '%s' é de preenchimento obrigatório, mas não foi informado.", ErroTipo.Erro, new string[0]),
            new Erro("VG0001", "A variável global '%s' é obrigatória, mas não foi informada.", ErroTipo.Erro, new string[0]),
            new Erro("SX0001", "Existe um ciclo de subprocessos na modelagem.", ErroTipo.Erro, new string[0])
        };

        public static Erro obter(string codigo, string[] valores)
        {
            foreach(Erro e in ListaErros)
            {
                if (e.Codigo == codigo)
                {
                    e.Descricao = String.Format(e.Descricao, valores);
                    e.Valores = valores;
                    return e;
                }
            }

            return new Erro("AA0000", "Erro desconhecido.", ErroTipo.Erro, new string[0]);
        }
    }

    public class Erros
    {
        public ObservableCollection<Erro> Lista { get; private set; }
        public int Quantidade
        {
            get => Lista.Count;
        }

        public Erros()
        {
            Lista = new ObservableCollection<Erro>();
        }

        public void Limpar()
        {
            Lista.Clear();
        }

        public bool Existe()
        {
            return (Lista.Count > 0);
        }

        public void Adicionar(string codigo, string[] valores)
        {
            Lista.Add(ErroCatalogo.obter(codigo, valores));
        }
    }
}