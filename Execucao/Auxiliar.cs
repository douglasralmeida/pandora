using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Execucao
{
    public class Auxiliar
    {
        public static (IntPtr, int) executarPrograma(string nomexe, string args, int sleep)
        {
            IntPtr handle;
            int id;
            string localExe;
            Process proc;
                        
            if (!File.Exists(nomexe))
                return (IntPtr.Zero, 0);

            proc = Process.Start(nomexe, args);
            Thread.Sleep(sleep);
            if (!Process.GetProcessesByName(proc.ProcessName).Any())
                return (IntPtr.Zero, 0);
            handle = proc.MainWindowHandle;
            id = proc.Id;

            return (handle, id);
        }

        public static void encerrarPrograma(int pid)
        {
            Process proc;

            proc = Process.GetProcessById(pid);
            proc.CloseMainWindow();
            proc.Close();
        }

        public static string obterDirSistema()
        {
            return Environment.SystemDirectory;
        }
    }
}
