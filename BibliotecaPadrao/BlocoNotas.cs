using Execucao;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BibliotecaPadrao
{
    class BlocoNotas : Modulo
    {
        const string exenome = "notepad.exe";

        private Funcao _funcaoAbrirPrograma = (ctes, args) =>
        {
            IntPtr handle;
            int pid;
            string localExe;
            
            localExe = Auxiliar.obterDirSistema() + "\\" + exenome;

            //** Incluir uma configuração para este número
            (handle, pid) = Auxiliar.executarPrograma(localExe, "", 3000);
            if (handle != IntPtr.Zero)
            {
                ctes.Add("handle", handle);
                ctes.Add("pid", pid);
                return (true, null);
            }
            else
            {
                return (false, "Bloco de Notas não foi aberto corretamente.");
            }
        };

        public BlocoNotas() : base("BlocoNotas")
        {

        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("AbrirPrograma", new FuncaoInfo(_funcaoAbrirPrograma, 0));
        }

        public override void adicionarConstNecessarias()
        {
            
        }
    }
}
