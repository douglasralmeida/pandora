using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        // arg1: Local do Executavel
        // arg2: Argumentos de execução
        private Funcao _funcaoAbrirPrograma = (ctes, args) =>
        {
            string argsChamada = "";
            IntPtr handle;
            string localExe;
            Process proc;

            using (var iter = args.GetEnumerator())
            {
                localExe = iter.Current.Valor;
                if (iter.MoveNext())
                    argsChamada = iter.Current.Valor;
            }
            if (!File.Exists(localExe))
                return (false, null);
            proc = Process.Start(localExe, argsChamada);
            Thread.Sleep(3000); //** Incluir uma configuração para este número
            if (!Process.GetProcessesByName(proc.ProcessName).Any())
                return (false, null);
            handle = proc.MainWindowHandle;
            if (handle != IntPtr.Zero)
            {
                ctes.Add("handle", handle);
                return (true, null);
            }
            else
                return (false, null);
        };

        private Funcao _funcaoDigitar = (ctes, args) =>
        {
            string texto = args.FirstOrDefault().Valor;
            dynamic handle;

            if (ctes.TryGetValue("handle", out handle))
            {
                IntPtr p = handle;
                SetForegroundWindow(p);
                System.Windows.Forms.SendKeys.SendWait(texto);
                return (true, null);
            }

            return (false, "Era esperado uma janela para enviar dados.");
        };

        // sem argumentos
        private Funcao _funcaoEncerrarPrograma = (ctes, args) =>
        {
            dynamic pid;

            if (ctes.TryGetValue("pid", out pid))
            {
                int id = pid;
                if (id == 0)
                    return (false, "Era esperado um programa para encerrar.");
                Auxiliar.encerrarPrograma(id);
                return (true, null);
            }

            return (false, "Era esperado um programa para fechar.");
        };

        public string Nome
        {
            get; set;
        }

        public Dictionary<string, FuncaoInfo> Funcoes { get; private set; }

        public Dictionary<string, ConstanteInfo> ConstantesNecessarias { get; private set; }

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
            //Funcoes.Add("AbrirPrograma", new FuncaoInfo(_funcaoAbrirPrograma, 2));
            Funcoes.Add("Digitar", new FuncaoInfo(_funcaoDigitar, 1));
            Funcoes.Add("FecharPrograma", new FuncaoInfo(_funcaoEncerrarPrograma, 0));
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
