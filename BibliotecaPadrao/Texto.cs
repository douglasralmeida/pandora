using Execucao;
using System;
using System.Collections.Generic;
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

        public Texto() : base("Texto")
        {

        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("Colar", new FuncaoInfo(_funcaoColar, 0));
            Funcoes.Add("Copiar", new FuncaoInfo(_funcaoCopiar, 0));
            Funcoes.Add("Exibir", new FuncaoInfo(_funcaoExibir, 0));
        }
    }
}