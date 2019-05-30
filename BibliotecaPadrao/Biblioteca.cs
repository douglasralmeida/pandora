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
        public static Modulo[] ObterTudo { get; } = new Modulo[] { new Plenus() };

        public static Modulo Obter(string Nome)
        {
            foreach (Modulo m in ObterTudo)
            {
                if (Nome == m.Nome)
                    return m;
            }

            return null;
        }
    }
}