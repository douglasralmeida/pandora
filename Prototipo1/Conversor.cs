using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Prototipo1
{
    class Conversor
    {
        private const string arqc = "caracinvalidos.txt";
        private const string arqs = "seqinvalidas.txt";
        private String caracteresInvalidos = "";
        private List<string> sequenciasInvalidas = new List<string>();

        public void carregarFiltros()
        {
            String[] caracs = File.ReadAllLines(arqc, new ASCIIEncoding());
            foreach (String carac in caracs)
                caracteresInvalidos += carac[0];

            var seqs = File.ReadAllLines(arqs, new ASCIIEncoding());
            foreach (string s in seqs)
                sequenciasInvalidas.Add(s);
        }

        public string processarPlenus(string texto)
        {
            StringBuilder builder = new StringBuilder(texto);
            String ns;

            foreach (string s in sequenciasInvalidas)
            {
                ns = "";

                foreach (char c in s)
                {
                    ns += ' ';
                }
                Debug.Print("Procurando por '" + s + "'...");
                builder.Replace(s, ns);
            }
            foreach (char c in caracteresInvalidos)
            {
                Debug.Print("Procurando por '" + c + "'...");
                builder.Replace(c, ' ');
            }

            return builder.ToString();
        }
    }
}