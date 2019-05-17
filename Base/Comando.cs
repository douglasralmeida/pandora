using System;
using System.Runtime.InteropServices;

namespace Base
{
    public class Comando
    {
        private const string TEC_ENTER = "{ENTER}";
        private const string TEC_TAB = "{TAB}";
        private IntPtr handle;

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(
            IntPtr hWnd);

        public Comando(IntPtr janela)
        {
            handle = janela;
        }

        public void enviar(String comando)
        {
            string texto = comando + TEC_ENTER;

            enviarTexto(comando);
        }
        public void enviarTexto(String texto)
        {
            SetForegroundWindow(handle);
            System.Diagnostics.Debug.Write(texto);
            System.Windows.Forms.SendKeys.SendWait(texto);
        }
        public void retornarAoInicio()
        {
            string comandos = TEC_TAB + "I" + TEC_ENTER;

            enviarTexto(comandos);
        }
    }
}