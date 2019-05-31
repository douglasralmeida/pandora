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
        public static bool PerguntaSimples(string mensagem)
        {
            Dialogo.TaskDialogButton botao;
            Dialogo.TaskDialog pergunta = new Dialogo.TaskDialog();
            DialogResult resultado;
            List<Dialogo.TaskDialogButton> botoes = new List<Dialogo.TaskDialogButton>();

            pergunta.MainInstruction = mensagem;
            pergunta.WindowTitle = "Pergunta";

            botao = new Dialogo.TaskDialogButton();
            botao.ButtonId = Convert.ToInt32(DialogResult.OK);
            botao.ButtonText = "Sim";
            botoes.Add(botao);

            botao = new Dialogo.TaskDialogButton();
            botao.ButtonId = Convert.ToInt32(DialogResult.Cancel);
            botao.ButtonText = "Não";
            botoes.Add(botao);

            pergunta.MainIcon = Dialogo.TaskDialogIcon.Information;
            pergunta.Buttons = botoes.ToArray();
            pergunta.UseCommandLinks = true;
            pergunta.DefaultButton = 0;
            resultado = (DialogResult)pergunta.Show();

            return resultado == DialogResult.OK;
        }
    }
}
