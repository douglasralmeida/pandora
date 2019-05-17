using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Prototipo1
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

        public static void clicar(IntPtr handle, int menuindex, int itemindex)
        {
            IntPtr item;
            IntPtr menu;
            IntPtr submenu;

            menu = GetMenu(handle);
            if (menu == IntPtr.Zero)
            {
                MessageBox.Show("O menu do Plenus não foi encontrado.");
            }
            submenu = GetSubMenu(menu, (UInt32)menuindex);
            if (submenu == IntPtr.Zero)
            {
                MessageBox.Show("O submenu 'Editar' do Plenus não foi encontrado.");
            }
            item = GetMenuItemID(submenu, (UInt32)itemindex);
            if (item == IntPtr.Zero)
            {
                MessageBox.Show("O item de menu 'Colar' do Plenus não foi encontrado.");
            }

            PostMessage(handle, WM_COMMAND, item, IntPtr.Zero);
        }
    }
}
