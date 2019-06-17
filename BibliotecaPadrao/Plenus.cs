using Execucao;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BibliotecaPadrao
{
    public class Plenus : Modulo
    {
        private Funcao _funcaoAutenticar = (ctes, args) =>
        {
            StringBuilder builder = new StringBuilder();
            string usuario;
            string matricula = "";
            string senha = "";
            dynamic handle;

            using (var iter = args.GetEnumerator())
            {
                usuario = iter.Current.Valor;
                if (iter.MoveNext())
                    matricula = iter.Current.Valor;
                if (iter.MoveNext())
                    senha = iter.Current.Valor;
            }
            if (ctes.TryGetValue("handle", out handle))
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

            return (false, null);
        };

        public Plenus() : base("Plenus")
        {
            
        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("Autenticar", new FuncaoInfo(_funcaoAutenticar, 3));
        }

        public override void adicionarConstNecessarias()
        {
            ConstantesNecessarias.Add("PLENUS_USUARIO", new ConstanteInfo("Usuário de acesso ao Plenus", true, false));
            ConstantesNecessarias.Add("PLENUS_MAT", new ConstanteInfo("Matrícula para acesso ao Plenus", true, false));
            ConstantesNecessarias.Add("PLENUS_SENHA", new ConstanteInfo("Senha de acesso ao Plenus", true, true));

            ConstantesNecessarias.Add("PLENUS_EXE", new ConstanteInfo("Caminho do arquivo executável do Plenus", false, true));
            ConstantesNecessarias.Add("PLENUS_CONFIG", new ConstanteInfo("Caminho do arquivo de configuração do Plenus", false, true));
        }
    }
}
