using System;
using System.Collections.Generic;
using System.Text;

namespace Execucao
{
    public class Variavel
    {
        public bool Opcional
        {
            get; set;
        }

        public bool Protegida
        {
            get; set;
        }

        public string Valor
        {
            get; set;
        }

        public Variavel(string valor)
        {
            Valor = valor;
            Opcional = false;
            Protegida = false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('"');
            if (Protegida)
            {
                foreach (char _ in Valor)
                {
                    builder.Append("■");
                }

            }
            else
            {
                builder.Append(Valor);
            };            
            builder.Append('"');

            return builder.ToString();
        }
    }
}
