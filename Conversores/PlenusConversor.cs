using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Conversores
{
    public class PlenusConversor : IConversor
    {
        private const string arqc = "caracinvalidos.txt";
        private const string arqs = "seqinvalidas.txt";
        private String caracteresInvalidos = "";
        private List<string> sequenciasInvalidas = new List<string>();

		public PlenusConversor()
        {
            var seqs = File.ReadAllLines(arqs, new ASCIIEncoding());
            String[] caracs = File.ReadAllLines(arqc, new ASCIIEncoding());

            foreach (String carac in caracs)
                caracteresInvalidos += carac[0];            
            foreach (string s in seqs)
                sequenciasInvalidas.Add(s);
        }

        public string processar(string texto)
        {
            StringBuilder builder = new StringBuilder(texto);
            String ns;

            foreach (string s in sequenciasInvalidas)
            {
                ns = "";
                foreach (char c in s)
                    ns += ' ';
                builder.Replace(s, ns);
            }
            foreach (char c in caracteresInvalidos)
                builder.Replace(c, ' ');

            return builder.ToString();
        }
    }
}