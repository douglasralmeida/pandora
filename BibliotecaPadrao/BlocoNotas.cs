using Execucao;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BibliotecaPadrao
{
    class BlocoNotas : Modulo
    {
        const string exenome = "notepad.exe";

        // sem argumentos
        // sem uso de constantes
        private Funcao _funcaoAbrirPrograma = (vars, args, opcoes) =>
        {
            IntPtr handle;
            int pid;
            string localExe;
            
            localExe = Auxiliar.obterDirSistema() + "\\" + exenome;

            //** Incluir uma configuração para este número
            (handle, pid) = Auxiliar.executarPrograma(localExe, "", "", 3000);
            if (handle != IntPtr.Zero)
            {
                vars.adicionar("bloconotas.handle", new Variavel(handle));
                vars.adicionar("bloconotas.pid", new Variavel(pid));
                return (true, null);
            }
            else
            {
                return (false, "Bloco de Notas não foi aberto corretamente.");
            }
        };

        // sem argumentos
        // sem uso de variáveis
        private Funcao _funcaoColar = (vars, args, opcoes) =>
        {
            dynamic handle;
            string texto = "^V";

            handle = vars.obterVar("bloconotas.handle");
            if (handle != null)
            {
                IntPtr p = handle;
                SetForegroundWindow(p);
                System.Windows.Forms.SendKeys.SendWait(texto);
                return (true, null);
            }

            return (false, "Era esperado uma janela para enviar comandos.");
        };

        // sem argumentos
        // sem uso de variáveis
        private Funcao _funcaoCopiar = (vars, args, opcoes) =>
        {
            dynamic handle;
            string texto = "^A^C";

            handle = vars.obterVar("bloconotas.handle");
            if (handle != null)
            {
                IntPtr p = handle;
                SetForegroundWindow(p);
                System.Windows.Forms.SendKeys.SendWait(texto);
                return (true, null);
            }

            return (false, "Era esperado uma janela para enviar comandos.");
        };

        // arg1 = texto a digitar na tela
        // usa a constante bloconotas.handle
        private Funcao _funcaoDigitar = (vars, args, opcoes) =>
        {
            IEnumerator<string> lista;
            string texto;
            dynamic handle;

            lista = args.GetEnumerator();
            if (lista.MoveNext())
                texto = lista.Current;
            else
                texto = "";
            handle = vars.obterVar("bloconotas.handle");
            if (handle != null)
            {
                IntPtr p = handle;
                SetForegroundWindow(p);
                System.Windows.Forms.SendKeys.SendWait(texto);
                return (true, null);
            }

            return (false, "Era esperado uma janela para enviar comandos.");
        };

        // sem argumentos
        // usa a constante bloconotas.pid
        private Funcao _funcaoEncerrarPrograma = (vars, args, opcoes) =>
        {
            dynamic pid;

            pid = vars.obterVar("bloconotas.pid");
            if (pid != null)
            {
                int id = pid;
                if (id == 0)
                    return (false, "Era esperado um PID válido de um programa para fechar.");
                Auxiliar.encerrarPrograma(id);
                return (true, null);
            }

            return (false, "Era esperado um programa para fechar.");
        };

        // sem argumentos
        // sem uso de variáveis
        private Funcao _funcaoLimpar = (vars, args, opcoes) =>
        {
            dynamic handle;
            string texto = "^A{Del}";

            handle = vars.obterVar("bloconotas.handle");
            if (handle != null)
            {
                IntPtr p = handle;
                SetForegroundWindow(p);
                System.Windows.Forms.SendKeys.SendWait(texto);
                return (true, null);
            }

            return (false, "Era esperado uma janela para enviar comandos.");
        };

        public BlocoNotas() : base("BlocoNotas")
        {

        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("AbrirPrograma", new FuncaoInfo(_funcaoAbrirPrograma, 0));
            Funcoes.Add("Colar", new FuncaoInfo(_funcaoColar, 0));
            Funcoes.Add("Copiar", new FuncaoInfo(_funcaoCopiar, 0));
            Funcoes.Add("Digitar", new FuncaoInfo(_funcaoDigitar, 1));
            Funcoes.Add("FecharPrograma", new FuncaoInfo(_funcaoEncerrarPrograma, 0));
            Funcoes.Add("Limpar", new FuncaoInfo(_funcaoLimpar, 1));
        }

        public override void adicionarConstNecessarias()
        {
            
        }
    }
}
