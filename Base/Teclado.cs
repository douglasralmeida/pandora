using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Base
{
    public static class Teclado
    {
        public static string TEC_ENTER = "TEC_ENTER";

        public static string TEC_TAB = "TEC_TAB";

        public static string TEC_RETURN = "TEC_RETURN";

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        public static void desabilitarCapsLock()
        {
            if (Control.IsKeyLocked(Keys.CapsLock))
            {
                const int KEYEVENTF_EXTENDEDKEY = 0x1;
                const int KEYEVENTF_KEYUP = 0x2;

                keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
                keybd_event(0x14, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
        }
    }
}
