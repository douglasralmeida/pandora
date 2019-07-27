using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Base
{
    public static class Parser
    {
        const string padrao = @"\((.*?) (.*?)\)";

        const string formato = "({0} {1})";

        //Transforma uma string no formato (ABC DEF) na própra em uma tupla ABC, DEF.
        // textoPuro: Se true, retorna o texto da entrada puro.
        //            Se false, retorna no formato '(AGF DEF)'.
        public static Tuple<string, string, string>[] analisar(string texto, bool textoPuro)
        {
            string chave, valor, variavel;
            Tuple<string, string, string> item;
            List<Tuple<string, string, string>> lista;
            MatchCollection combinacoes;

            combinacoes = Regex.Matches(texto, padrao);
            lista = new List<Tuple<string, string, string>>();
            foreach (Match m in combinacoes)
            {
                valor = m.Value;
                chave = m.Groups[1].ToString();
                if (textoPuro)                   
                    variavel = m.Groups[2].ToString();
                else
                    variavel = string.Format(formato, chave, m.Groups[2]);
                item = new Tuple<string, string, string>(valor, chave, variavel);
                lista.Add(item);
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
