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

        private Funcao _funcaoMesclarArquivosPDF = (vars, args, opcoes) =>
        {
            List<string> arquivos;
            string arquivosaida;
            dynamic dados;
            string dirtrabalho;

            if (args.Cont < 3)
                return (false, "A operação MesclarArquivosPDF esperava 3 argumentos, mas eles não foram encontrados.");
            dados = vars.obterVar("global.dirtrabalho");
            if (dados != null)
                dirtrabalho = dados;
            else
                dirtrabalho = "";
            arquivos = new List<string>();
            foreach (string item in args)
                arquivos.Add(dirtrabalho + item);
            arquivosaida = arquivos[arquivos.Count - 1];
            arquivos.RemoveAt(arquivos.Count - 1);

            PDF.mesclar(arquivos.ToArray(), arquivosaida);

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

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("MesclarArquvosPDF", new FuncaoInfo(_funcaoMesclarArquivosPDF, 3));
        }
    }
}
