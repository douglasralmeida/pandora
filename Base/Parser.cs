using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Base
{
    public static class Parser
    {
        //const string padraoEntrada = "{ENTRADA (.*?)}";

        const string padraoEntrada = @"\(ENTRADA (.*?)\)";

        const string padraoEntradaGen = "(ENTRADA {0})";

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
                {
                    s = m.Groups[1].ToString();
                }
                else
                {
                    s = string.Format(padraoEntradaGen, m.Groups[1]);
                }
                lista.Add(s);
            }
            return lista.ToArray();
        }

        // usar sep = SPACE e escape = "

        public static string[] dividirString(string texto, char sep, char escape)
        {
            int quantidade = texto.Count(c => c == escape);

            if (quantidade % 2 != 0) // número de caracteres de esacpe impar
            {
                int ultimoIndice = texto.LastIndexOf("" + escape, StringComparison.Ordinal);
                texto = texto.Remove(ultimoIndice, 1); // remove o última caractere de escape
            }

            var resultado = texto.Split(escape)
                                      .Select((element, index) => index % 2 == 0  // se indice par
                                           ? element.Split(new[] { sep }, StringSplitOptions.RemoveEmptyEntries)  // divide o item
                                           : new string[] { element })  // mantem o item inteiro
                                      .SelectMany(element => element).ToList();

            return resultado.ToArray();
        }
    }
}
