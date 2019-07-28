using Dialogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Windows.Media;

namespace Modelagem
{
    public static class CaixaDialogo
    {
        private static TaskDialog criarDialogo(string textoPadrao)
        {
            TaskDialog dialogo = new TaskDialog();

            dialogo.MainInstruction = textoPadrao;
            dialogo.DefaultButton = 0;
            dialogo.PositionRelativeToWindow = true;

            return dialogo;
        }

        private static void exibirErro(IntPtr ownerhandle, string mensagem)
        {
            TaskDialogCommonButtons botoes = 0;
            TaskDialog erroMensagem = criarDialogo(mensagem);

            erroMensagem.MainIcon = TaskDialogIcon.Error;
            erroMensagem.WindowTitle = "Erro";
            erroMensagem.UseCommandLinks = false;

            botoes |= TaskDialogCommonButtons.Ok;
            erroMensagem.CommonButtons = botoes;

            erroMensagem.Show(ownerhandle);
        }

        private static bool exibirPergunta(IntPtr ownerhandle, string mensagem)
        {
            TaskDialogButton botao;
            TaskDialog pergunta = criarDialogo(mensagem);
            DialogResult resultado;
            List<Dialogo.TaskDialogButton> botoes = new List<Dialogo.TaskDialogButton>();

            pergunta.MainIcon = TaskDialogIcon.Information;
            pergunta.WindowTitle = "Pergunta";
            pergunta.UseCommandLinks = true;

            botao = new TaskDialogButton();
            botao.ButtonId = Convert.ToInt32(DialogResult.OK);
            botao.ButtonText = "Sim";
            botoes.Add(botao);

            botao = new TaskDialogButton();
            botao.ButtonId = Convert.ToInt32(DialogResult.Cancel);
            botao.ButtonText = "Não";
            botoes.Add(botao);

            pergunta.Buttons = botoes.ToArray();

            resultado = (DialogResult)pergunta.Show(ownerhandle);

            return resultado == DialogResult.OK;
        }

        public static bool PerguntaSimples(Window janela, string mensagem)
        {
            IntPtr handle = new WindowInteropHelper(janela).Handle;

            return exibirPergunta(handle, mensagem);
        }

        public static bool PerguntaSimples(System.Windows.Controls.UserControl controle, string mensagem)
        {
            Window janelapai = Window.GetWindow(controle);
            IntPtr handle = new WindowInteropHelper(janelapai).Handle;

            return exibirPergunta(handle, mensagem);
        }

        public static void ErroSimples(System.Windows.Controls.UserControl controle, string mensagem)
        {
            Window janelapai = Window.GetWindow(controle);
            IntPtr handle = new WindowInteropHelper(janelapai).Handle;

            exibirErro(handle, mensagem);
        }

        public static void ErroSimples(Window janela, string mensagem)
        {
            IntPtr handle = new WindowInteropHelper(janela).Handle;

            exibirErro(handle, mensagem);
        }
    }
}
