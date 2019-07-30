using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace Base
{
    public static class Menu
    {
        private const uint WM_COMMAND = 0x0111;

        [DllImport("user32.dll")]
        private static extern IntPtr GetMenu(
            IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetSubMenu(
            IntPtr hMenu,
            UInt32 nPos);

        [DllImport("user32.dll")]
        private static extern IntPtr GetMenuItemID(
            IntPtr hWnd,
            UInt32 nPos);

        [DllImport("user32.dll")]
        private static extern IntPtr PostMessage(
            IntPtr hWnd,
            UInt32 wMsg,
            IntPtr wParam,
            IntPtr lParam);

        // simula um clique de menu
        // retorna 0 se houve sucesso
        // retorna 1 se o menu não foi encontrado
        // retorna 2 se o submenu não foi encontrado
        // retorna 3 se o item de menu não foi encontrado.
        public static int clicar(IntPtr handle, int menuindex, int itemindex)
        {
            IntPtr item;
            IntPtr menu;
            IntPtr submenu;

            menu = GetMenu(handle);
            if (menu == IntPtr.Zero)
                return 1;
            submenu = GetSubMenu(menu, (UInt32)menuindex);
            if (submenu == IntPtr.Zero)
                return 2;
            item = GetMenuItemID(submenu, (UInt32)itemindex);
            if (item == IntPtr.Zero)
                return 3;
            PostMessage(handle, WM_COMMAND, item, IntPtr.Zero);

            return 0;
        }
    }
}
