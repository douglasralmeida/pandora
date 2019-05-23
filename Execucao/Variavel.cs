using System;
using System.Collections.Generic;
using System.Text;

namespace Execucao
{
    class Variavel
    {
        public string Nome
        {
            get; set;
        }

        public bool Opcional
        {
            get; set;
        }

        public var Valor
        {
            get; set;
        }

        public Variavel(string nome, var valor)
        {
            Nome = nome;
            Valor = valor;
            Opcional = false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder(Valor);

            //builder.Append('"', 0, 1);
            //builder.Append('"');

            return builder.ToString();
        }
    }
}
