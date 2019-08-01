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
        public static (IntPtr, int) executarPrograma(string nomeexe, string args, string workdir, int sleep)
        {
            IntPtr handle;
            int id;
            Process proc;
            ProcessStartInfo startinfo;
                        
            if (!File.Exists(workdir + nomeexe))
                return (IntPtr.Zero, 0);

            startinfo = new ProcessStartInfo(nomeexe, args);
            startinfo.WorkingDirectory = workdir;
            proc = Process.Start(startinfo);
            Thread.Sleep(sleep);
            proc.Refresh();
            if (proc.HasExited)
                return (IntPtr.Zero, 0);
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
