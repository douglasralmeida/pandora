using Base;
using Execucao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BibliotecaPadrao
{
    public class Plenus : Modulo
    {
        private Funcao _funcaoAbrirPrograma = (vars, args) =>
        {
            IntPtr handle;
            int pid;
            dynamic saida;
            string localarquivos;
            string nomeexe;
            string nomeconfig;

            saida = vars.obterVar("PLENUS_EXE");
            if (saida != null)
                nomeexe = saida;
            else
                return (false, "Uma variável global era esperada, mas não foi encontrada.");
            saida = vars.obterVar("PLENUS_CONFIG");
            if (saida != null)
                nomeconfig = saida;
            else
                return (false, "Uma variável global era esperada, mas não foi encontrada.");
            saida = vars.obterVar("PLENUS_LOCAL");
            if (saida != null)
                localarquivos = saida;
            else
                return (false, "Uma variável global era esperada, mas não foi encontrada.");

            //** Incluir uma configuração para este número
            (handle, pid) = Auxiliar.executarPrograma(nomeexe, nomeconfig, localarquivos, 3000);
            if (handle != IntPtr.Zero)
            {
                vars.adicionar("plenus.handle", new Variavel(handle));
                vars.adicionar("plenus.pid", new Variavel(pid));
                return (true, null);
            }
            else
            {
                return (false, "O Plenus não foi aberto corretamente.");
            }
        };

        // sem argumentos
        // usa variáveis: PLENUS_USUARIO, PLENUS_MAT, PLENUS_SENHA, plenus.handle
        private Funcao _funcaoAutenticar = (vars, args) =>
        {
            StringBuilder builder = new StringBuilder();
            dynamic saida;
            string usuario;
            string matricula = "";
            string senha = "";
            dynamic handle;

            saida = vars.obterVar("PLENUS_USUARIO");
            if (saida != null)
                usuario = saida;
            else
                return (false, "A variável global 'PLENUS_USUARIO' era esperada, mas não foi encontrada.");
            saida = vars.obterVar("PLENUS_MAT");
            if (saida != null)
                matricula = saida;
            else
                return (false, "A variável global 'PLENUS_MAT' era esperada, mas não foi encontrada.");
            saida = vars.obterVar("PLENUS_SENHA");
            if (saida != null)
                senha = saida;
            else
                return (false, "A variável global 'PLENUS_SENHA' era esperada, mas não foi encontrada.");
            handle = vars.obterVar("plenus.handle");
            if (handle != null)
            {
                IntPtr p = handle;
                SetForegroundWindow(p);

                builder.Append(usuario);
                builder.Append(Teclado.TEC_ENTER);
                System.Windows.Forms.SendKeys.SendWait(builder.ToString());
                Thread.Sleep(1000);
                builder.Clear();

                builder.Append(matricula);
                builder.Append(Teclado.TEC_TAB);
                builder.Append(senha);
                builder.Append(Teclado.TEC_ENTER);
                System.Windows.Forms.SendKeys.SendWait(builder.ToString());

                return (true, null);
            }

            return (false, "Uma janela do Plenus era esperada, mas não foi encontrada.");
        };

        // sem argumentos
        // não usa variáveis
        private Funcao _funcaoCopiarTela = (vars, args) =>
        {
            int resultado;
            dynamic handle;

            handle = vars.obterVar("plenus.handle");
            if (handle != null)
            {
                //clicar em Editar->Sel. Tudo
                resultado = Base.Menu.clicar(handle, 2, 3);
                if (resultado != 0)
                    return (false, "Erro.");

                //clicar em Editar->Copiar
                resultado = Base.Menu.clicar(handle, 2, 1);
                if (resultado != 0)
                    return (false, "Erro.");

                //dorme 1 seg aguardando processamento
                System.Threading.Thread.Sleep(1000);

                return (true, null);
            }

            return (false, "Uma janela do Plenus era esperada, mas não foi encontrada.");
        };

        // arg1 = texto a digitar na tela
        // usa a variável plenus.handle
        private Funcao _funcaoDigitar = (vars, args) =>
        {
            IEnumerator<string> lista;
            string texto;
            dynamic handle;

            if (args.Cont < 1)
                return (false, "A operação Digitar esperava 1 argumento, mas ele não foi encontrado.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            texto = lista.Current;
            handle = vars.obterVar("plenus.handle");
            if (handle != null)
            {
                IntPtr p = handle;
                SetForegroundWindow(p);
                System.Windows.Forms.SendKeys.SendWait(texto);
                return (true, null);
            }

            return (false, "Uma janela do Plenus era esperada, mas não foi encontrada.");
        };

        public Plenus() : base("Plenus")
        {
            
        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("AbrirPrograma", new FuncaoInfo(_funcaoAbrirPrograma, 0));
            Funcoes.Add("Autenticar", new FuncaoInfo(_funcaoAutenticar, 0));
            Funcoes.Add("CopiarTela", new FuncaoInfo(_funcaoDigitar, 0));
            Funcoes.Add("Digitar", new FuncaoInfo(_funcaoDigitar, 1));
        }

        public override void adicionarConstNecessarias()
        {
            ConstantesNecessarias.Add("PLENUS_USUARIO", new ConstanteInfo("Usuário de acesso ao Plenus", true, false, true));
            ConstantesNecessarias.Add("PLENUS_MAT", new ConstanteInfo("Matrícula para acesso ao Plenus", true, false, true));
            ConstantesNecessarias.Add("PLENUS_SENHA", new ConstanteInfo("Senha de acesso ao Plenus", true, true, true));

            ConstantesNecessarias.Add("PLENUS_EXE", new ConstanteInfo("Nome do arquivo executável do Plenus", false, true, true));
            ConstantesNecessarias.Add("PLENUS_CONFIG", new ConstanteInfo("Nome do arquivo de configuração do Plenus", false, true, true));
            ConstantesNecessarias.Add("PLENUS_LOCAL", new ConstanteInfo("Caminho dos arquivos do Plenus", false, true, true));
        }
    }
}
