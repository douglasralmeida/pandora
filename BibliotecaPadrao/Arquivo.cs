using Execucao;
using System;
using System.Collections.Generic;
using System.Text;

namespace BibliotecaPadrao
{
    public class Arquivo : Modulo
    {
        private Funcao _funcaoCopiarArquivo = (vars, args, opcoes) =>
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

        private Funcao _funcaoExcluirArquivo = (vars, args, opcoes) =>
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

        private Funcao _funcaoMesclarArquivos = (vars, args, opcoes) =>
        {
            IEnumerator<string> lista;
            string arquivosaida;
            dynamic dados;
            string dirtrabalho;
            List<string> listaarquivos;
            string nomevar;
            string texto;

            if (args.Cont < 2)
                return (false, "A operação MesclarArquivos esperava 2 argumentos, mas eles não foram encontrados.");
            listaarquivos = new List<string>();
            dados = vars.obterVar("global.dirtrabalho", false);
            if (dados != null)
                dirtrabalho = dados;
            else
                dirtrabalho = "";
            lista = args.GetEnumerator();
            lista.MoveNext();
            nomevar = lista.Current;
            lista.MoveNext();
            arquivosaida = lista.Current;
            dados = vars.obterVar(nomevar, false);
            if (dados != null)
            {
                texto = dados;
                foreach(string s in texto.Split(' '))
                    listaarquivos.Add(dirtrabalho + s);
            }
            PDF.mesclar(listaarquivos.ToArray(), arquivosaida);

            return (true, "");
        };

        private Funcao _funcaoMoverArquivo = (vars, args, opcoes) =>
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

        private Funcao _funcaoRenomearArquivo = (vars, args, opcoes) =>
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

        public Arquivo() : base("Arquivo")
        {
            
        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("MesclarArquvos", new FuncaoInfo(_funcaoMesclarArquivos, 3));
        }
    }
}