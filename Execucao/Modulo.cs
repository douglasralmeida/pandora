using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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

        public string Nome
        {
            get; set;
        }

        public Dictionary<string, FuncaoInfo> Funcoes { get; private set; }

        public Dictionary<string, ConstanteInfo> ConstantesNecessarias { get; private set; }

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

        // sem argumentos
        // usa uma variável: Global.DirTrabalho
        private Funcao _funcaoExibirDirTrabalho = (vars, args, opcoes) =>
        {
            dynamic dados;

            dados = vars.obterVar("global.dirtrabalho");
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

        public Modulo(string nome)
        {
            ConstantesNecessarias = new Dictionary<string, ConstanteInfo>();
            Funcoes = new Dictionary<string, FuncaoInfo>();
            adicionarComandos();
            adicionarConstNecessarias();
            Nome = nome;
        }

        public virtual void adicionarComandos()
        {
            Funcoes.Add("DefinirIntervaloExecucao", new FuncaoInfo(_funcaoDefinirIntervaloExecucao, 1));
            Funcoes.Add("ExibirDiretorioTrabalho", new FuncaoInfo(_funcaoExibirDirTrabalho, 0));
        }

        public virtual void adicionarConstNecessarias()
        {

        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
