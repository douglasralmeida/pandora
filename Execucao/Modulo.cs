using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Execucao
{
    /* CONSTANTEINFO
     * descricao  = Descrição da constante
     * oculta     = Trata a constante como senha
     * individual = Constante dever ser salva numa carteira
     */
    public struct ConstanteInfo
    {
        public string descricao;
        public bool individual;
        public bool obrigatoria;
        public bool oculta;

        public ConstanteInfo(string desc, bool indiv, bool oc, bool ob)
        {
            descricao = desc;
            individual = indiv;
            oculta = oc;
            obrigatoria = ob;
        }
    }

    public struct FuncaoInfo {
        public Funcao funcao;
        public int numArgumentos;

        public FuncaoInfo(Funcao _func, int numArg)
        {
            funcao = _func;
            numArgumentos = numArg;
        }
    }

    public class Modulo
    {
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);

        public string Nome { get; set; }

        public SortedDictionary<string, FuncaoInfo> Funcoes { get; private set; }

        public Dictionary<string, ConstanteInfo> ConstantesNecessarias { get; private set; }

        //usa dois argumentos
        //usa variáveis
        private Funcao _funcaoAdicionarNaLista = (vars, args, opcoes) =>
        {
            dynamic dados;
            List<string> lista;
            string nomevar;
            string valoratual;

            if (args.Cont < 2)
                return (false, "A operação AdicionarNaLista esperava 2 argumentos, mas eles não foram encontrados.");
            lista = new List<string>();
            foreach (string s in args)
                    lista.Add("\"" + s + "\"");
            nomevar = lista[0].Remove(0, 1);
            nomevar = nomevar.Remove(nomevar.Length - 1);
            lista.RemoveAt(0);
            dados = vars.obterVar(nomevar, true);
            if (dados == null)
                dados = "";
            valoratual = dados;
            foreach (string s in lista)
                valoratual += ' ' + s;
            vars.adicionar(nomevar, true, new Variavel(valoratual));

            return (true, null);
        };
        
        // usa um argumento
        // não usa variáveis
        private Funcao _funcaoDefinirIntervaloExecucao = (vars, args, opcoes) =>
        {
            IEnumerator<string> lista;
            string intervalo;

            if (args.Cont < 1)
                return (false, "A operação DefinirIntervaloExecucao esperava 1 argumento, mas ele não foi encontrado.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            intervalo = lista.Current;
            opcoes.Atraso = int.Parse(intervalo);

            return (true, null);
        };

        // usa um argumento
        // usa uma variável: Global.DirTrabalho
        private Funcao _funcaoDefinirDirTrabalho = (vars, args, opcoes) =>
        {
            IEnumerator<string> lista;
            string dir;

            if (args.Cont < 1)
                return (false, "A operação DefinirDiretorioTrabalho esperava 1 argumento, mas ele não foi encontrado.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            dir = lista.Current;
            vars.adicionar("global.dirtrabalho", false, new Variavel(dir));

            return (true, null);
        };

        // sem argumentos
        // usa uma variável: Global.DirTrabalho
        private Funcao _funcaoExibirDirTrabalho = (vars, args, opcoes) =>
        {
            dynamic dados;

            dados = vars.obterVar("global.dirtrabalho", false);
            if (dados != null)
            {
                ProcessStartInfo si = new ProcessStartInfo(dados);
                si.UseShellExecute = true;
                try
                {
                    Process.Start(si);
                }
                catch (Win32Exception we)
                {
                    return (false, "Falha na execução da operação ExibirDiretorioTrabalho: " + we.Message);
                }
            }

            return (true, null);
        };

        // sem argumentos
        // usa uma variável: Global.DirTrabalho
        private Funcao _funcaoLimparDirTrabalho = (vars, args, opcoes) =>
        {
            dynamic dados;

            dados = vars.obterVar("global.dirtrabalho", false);
            if (dados != null)
            {
                DirectoryInfo di = new DirectoryInfo(dados);
                foreach (FileInfo arquivo in di.EnumerateFiles())
                    arquivo.Delete();
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                    dir.Delete(true);
            }

            return (true, null);
        };

        public Modulo(string nome)
        {
            ConstantesNecessarias = new Dictionary<string, ConstanteInfo>();
            Funcoes = new SortedDictionary<string, FuncaoInfo>();
            adicionarComandos();
            adicionarConstNecessarias();
            Nome = nome;
        }

        public virtual void adicionarComandos()
        {
            Funcoes.Add("AdicionarNaLista", new FuncaoInfo(_funcaoAdicionarNaLista, 2));
            Funcoes.Add("DefinirIntervaloExecucao", new FuncaoInfo(_funcaoDefinirIntervaloExecucao, 1));
            Funcoes.Add("DefinirDiretorioTrabalho", new FuncaoInfo(_funcaoDefinirDirTrabalho, 1));
            Funcoes.Add("ExibirDiretorioTrabalho", new FuncaoInfo(_funcaoExibirDirTrabalho, 0));
            Funcoes.Add("LimparDiretorioTrabalho", new FuncaoInfo(_funcaoLimparDirTrabalho, 0));
        }

        public virtual void adicionarConstNecessarias()
        {
            ConstantesNecessarias.Add("USUARIO_MAT", new ConstanteInfo("Matrícula para acesso ao sistemas do INSS", true, false, true));
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
