using Execucao;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaPadrao
{
    public class Arquivo : Modulo
    {
        private Funcao _funcaoAbrirArquivo = (vars, args, opcoes) =>
        {
            IEnumerator<string> lista;
            string nomearquivo;

            if (args.Cont < 1)
                return (false, "A operação AbrirArquivo esperava 1 argumento, mas ele não foi encontrado.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            nomearquivo = lista.Current;

            return (true, "");
        };

        private Funcao _funcaoMesclarArquivo = (vars, args, opcoes) =>
        {
            IEnumerator<string> lista;
            string nomearquivo;

            if (args.Cont < 1)
                return (false, "A operação MesclarArquivo esperava 2 argumentos, mas eles não foram encontrados.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            nomearquivo = lista.Current;

            return (true, "");
        };

        private Funcao _funcaoSalvarArquivo = (vars, args, opcoes) =>
        {
            IEnumerator<string> lista;
            string nomearquivo;

            if (args.Cont < 1)
                return (false, "A operação SalvarArquivo esperava 1 argumento, mas ele não foi encontrado.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            nomearquivo = lista.Current;

            return (true, "");
        };


        public Arquivo() : base("Arquivo")
        {

        }
    }
}
