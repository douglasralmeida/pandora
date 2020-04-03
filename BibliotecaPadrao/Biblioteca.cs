using Execucao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaPadrao
{
    public static class Biblioteca
    {
        public static Modulo[] ObterTudo { get; } = new Modulo[] { new BlocoNotas(), new Plenus(), new Texto() };

        public static Modulo Obter(string Nome)
        {
            foreach (Modulo m in ObterTudo)
            {
                if (Nome == m.Nome)
                    return m;
            }

            return null;
        }

        public static KeyValuePair<string, ConstanteInfo>[] obterCtesChaves()
        {
            List<KeyValuePair<string, ConstanteInfo>> resultado = new List<KeyValuePair<string, ConstanteInfo>>();
            foreach (Modulo m in ObterTudo)
            {
                foreach (KeyValuePair<string, ConstanteInfo> par in m.ConstantesNecessarias)
                {
                    resultado.Add(par);
                }
            }

            return resultado.ToArray();
        }

        public static string[] obterCtesString()
        {
            List<string> resultado = new List<string>();
            foreach(Modulo m in ObterTudo)
            {
                foreach(string s in m.ConstantesNecessarias.Keys)
                {
                    if (!resultado.Contains(s))
                        resultado.Add(s);
                }
            }

            return resultado.ToArray();
        }
    }
}