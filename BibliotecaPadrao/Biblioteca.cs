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
        static readonly Modulo[] modulos = new Modulo[] { new Plenus() };

        public static Modulo[] ObterTudo
        {
            get
            {
                return modulos;
            }
        }

        public static Modulo Obter(string Nome)
        {
            foreach (Modulo m in modulos)
            {
                if (Nome == m.Nome)
                    return m;
            }

            return null;
        }
    }
}