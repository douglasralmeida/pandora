using Dialogo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Modelagem
{
    public static class CaixaDialogo
    {
        private static TaskDialog criarDialogo(string textoPadrao)
        {
            TaskDialog dialogo = new TaskDialog();

            dialogo.MainInstruction = textoPadrao;
            dialogo.DefaultButton = 0;

            return dialogo;
        }

        public static bool PerguntaSimples(string mensagem)
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

            resultado = (DialogResult)pergunta.Show();

            return resultado == DialogResult.OK;
        }

        public static void ErroSimples(string mensagem)
        {
            TaskDialogCommonButtons botoes = 0;
            TaskDialog erroMensagem = criarDialogo(mensagem);

            erroMensagem.MainIcon = TaskDialogIcon.Error;
            erroMensagem.WindowTitle = "Erro";
            erroMensagem.UseCommandLinks = false;

            botoes |= TaskDialogCommonButtons.Ok;
            erroMensagem.CommonButtons = botoes;

            erroMensagem.Show();
        }
    }
}
