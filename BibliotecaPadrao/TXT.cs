using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BibliotecaPadrao
{
    public static class TXT
    {
        public static void substituirTexto(string arquivoorigem, string palavrachave, string arquivodestino, string substituto)
        {
            StreamReader reader = new StreamReader(arquivoorigem);
            string input = reader.ReadToEnd();

            using (StreamWriter writer = new StreamWriter(arquivodestino, true))
            {
                {
                    string output = input.Replace(palavrachave, substituto);
                    writer.Write(output);
                }
                writer.Close();
            }
        }

        public static void gerarDeTexto(string texto, string nomearquivo)
        {
            System.IO.File.WriteAllText(nomearquivo, texto);
        }
    }
}
