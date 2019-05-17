using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Prototipo1
{
    class Plenus
    {
        Conversor conversor;
        private IntPtr handlePai = IntPtr.Zero;
        private IntPtr handleTerminal = IntPtr.Zero;
        private string telaObtida;

        public Plenus()
        {
            conversor = new Conversor();
            conversor.carregarFiltros();
        }

        public void inserirTexto(string texto)
        {
            Clipboard.SetText(texto);
            Clipboard.Flush();
            menuSimular(2, 2); //clicar em Editar->Copiar
        }

        public bool encontrarJanela()
        {
            handlePai = UI.encontrarJanela(IntPtr.Zero, "OWL_Window");
            if (handlePai == IntPtr.Zero)
                return false;
            handleTerminal = UI.encontrarJanela(handlePai, null);
            if (handleTerminal == IntPtr.Zero)
                return false;
            handleTerminal = UI.encontrarJanela(handleTerminal, null);
            if (handleTerminal == IntPtr.Zero)
                return false;
            handleTerminal = UI.encontrarJanela(handleTerminal, null);

            return handleTerminal != IntPtr.Zero;
        }

        private void menuSimular(int submenu, int itemmenu)
        {
            Menu.clicar(handlePai, submenu, itemmenu);
        }

        public void obterTextoTela()
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            String texto;

            menuSimular(2, 3); //clicar em Editar->Sel. Tudo
            menuSimular(2, 1); //clicar em Editar->Copiar
            System.Threading.Thread.Sleep(1000); //dormir 1 segundo
            texto = Clipboard.GetData("Text").ToString();
            telaObtida = ascii.GetString((new UnicodeEncoding()).GetBytes(texto));
        }

        public string textoObtido()
        {
            return processarTexto(telaObtida);
        }

        private string processarTexto(string texto)
        {
            return conversor.processarPlenus(texto);
        }
    }
}