using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    static class WinAPI
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowA(
            string lpClassName,
            string lpWindowName);

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindowExA(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
            string lpszWindow);

        public static IntPtr encontrarJanela(IntPtr handle, String classe)
        {
            if (handle == IntPtr.Zero)
            {
                return FindWindowExA(handle, IntPtr.Zero, classe, null);
            }
            else
            {
                return FindWindowA(classe, null);
            }
        }
    }
}
