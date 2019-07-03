using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Base
{
    public static class Parser
    {
        const string padraoEntrada = "{ENTRADA (.*?)}";
        const string padraoEntradaGen = "{{ENTRADA {0}}}";

        public static string[] analisarVarEntrada(string texto, bool textoPuro)
        {
            string s;
            List<string> lista;
            MatchCollection combinacoes;

            combinacoes = Regex.Matches(texto, padraoEntrada);
            lista = new List<string>();
            foreach (Match m in combinacoes)
            {
                if (textoPuro)
                    s = m.Groups[1].ToString();
                else
                    s = string.Format(padraoEntradaGen, m.Groups[1]);
                lista.Add(s);
            }
            return lista.ToArray();
        }
    }
}
