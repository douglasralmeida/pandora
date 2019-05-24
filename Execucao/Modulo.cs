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
    public class Modulo
    {
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);

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

            return (false, null);
        };

        private Dictionary<String, Funcao> _funcoes;

        public string Nome
        {
            get; set;
        }

        public Dictionary<String, Funcao> Funcoes
        {
            get
            {
                return _funcoes;
            }
        }

        protected Modulo()
        {
            _funcoes = new Dictionary<string, Funcao>();
            adicionarComandos();
        }

        public Modulo(string nome)
        {
            _funcoes = new Dictionary<string, Funcao>();
            adicionarComandos();
            Nome = nome;
        }

        public virtual void adicionarComandos()
        {
            _funcoes.Add("AbrirPrograma", _funcaoAbrirPrograma);
            _funcoes.Add("Digitar", _funcaoDigitar);
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
