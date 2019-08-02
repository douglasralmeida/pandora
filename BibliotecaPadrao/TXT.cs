using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BibliotecaPadrao
{
    public static class TXT
    {
        public static void gerarDeTexto(string texto, string nomearquivo)
        {
            System.IO.File.WriteAllText(nomearquivo, texto);
        }
    }
}
