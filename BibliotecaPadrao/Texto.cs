using Execucao;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace BibliotecaPadrao
{
    public class Texto : Modulo
    {
        // sem argumentos
        // sem uso de variáveis
        // gera uma variável: Texto.Variavel
        private Funcao _funcaoColar = (vars, args) =>
        {
            string dados = "";

            dados = Clipboard.GetText();
            vars.adicionar("texto.variavel", new Variavel(dados));

            return (true, null);
        };

        // sem argumentos
        // usa uma variável: Texto.Variavel
        private Funcao _funcaoCopiar = (vars, args) =>
        {
            dynamic dados;

            dados = vars.obterVar("texto.variavel");
            if (dados != null)
            {
                Clipboard.SetText(dados);
            }

            return (true, null);
        };

        // usa um argumentos texto
        // sem uso de variáveis
        // gera uma variável: Texto.Variavel
        private Funcao _funcaoDefinir = (vars, args) =>
        {
            string texto = "";
            IEnumerator<string> lista;

            if (args.Cont < 1)
                return (false, "A operação Definir esperava 1 argumento, mas ele não foi encontrado.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            texto = lista.Current;
            vars.adicionar("texto.variavel", new Variavel(texto));

            return (true, null);
        };

        // sem argumentos
        // usa uma variável: Texto.Variavel
        private Funcao _funcaoExibir = (vars, args) =>
        {
            dynamic dados;

            dados = vars.obterVar("texto.variavel");
            if (dados != null)
            {
                MessageBox.Show(dados);
            }

            return (true, null);
        };

        // sem argumentos
        // usa uma variável: Texto.Variavel
        private Funcao _funcaoLimpar = (vars, args) =>
        {
            vars.adicionar("texto.variavel", new Variavel(""));
            return (true, null);
        };

        // dois argumentos: tipoextensao, nomearquivo
        // usa variáveis: global.dirtrabalho, texto.variavel
        private Funcao _funcaoSalvar = (vars, args) =>
        {
            IEnumerator<string> lista;
            bool extok = false;
            string[] extaceitaveis = { "pdf", "txt" };
            dynamic dados;
            string dirtrabalho;
            string nomearquivo;
            string ext;

            if (args.Cont < 2)
                return (false, "A operação SalvarTela esperava 2 argumentos, mas eles não foram encontrados.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            ext = lista.Current;
            foreach (string s in extaceitaveis)
                extok |= (s == ext);
            if (!extok)
                return (false, "O primeiro parâmetro da operação SalvarTela está incorreto. Era esperado: pdf ou txt.");
            lista.MoveNext();
            nomearquivo = lista.Current;
            dirtrabalho = vars.obterVar("global.dirtrabalho");
            if (dirtrabalho == null)
                return (false, "O diretório de trabalho do Pandora não foi configurado.");
            dados = vars.obterVar("texto.variavel");
            //salva em um arquivo PDF
            if (dados != null)
            {
                try
                {
                    if (ext == "pdf")
                        PDF.gerarDeTexto(dados, dirtrabalho + nomearquivo);
                    else
                        TXT.gerarDeTexto(dados, dirtrabalho + nomearquivo);
                }
                catch (DirectoryNotFoundException)
                {
                    return (false, "O diretório de trabalho especificado está inacessível ou não existe.");
                }
                return (true, null);
            }

            return (false, "Não há nada para salvar.");
        };

        public Texto() : base("Texto")
        {

        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("Colar", new FuncaoInfo(_funcaoColar, 0));
            Funcoes.Add("Copiar", new FuncaoInfo(_funcaoCopiar, 0));
            Funcoes.Add("Definir", new FuncaoInfo(_funcaoCopiar, 1));
            Funcoes.Add("Exibir", new FuncaoInfo(_funcaoExibir, 0));
            Funcoes.Add("Limpar", new FuncaoInfo(_funcaoLimpar, 0));
            Funcoes.Add("Salvar", new FuncaoInfo(_funcaoSalvar, 2));
        }
    }
}