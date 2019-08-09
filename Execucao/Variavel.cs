using System;
using System.Collections.Generic;
using System.Text;

namespace Execucao
{
    public class Variaveis
    {
        public Dictionary<string, Variavel> Lista { get; }

        public Variaveis()
        {
            Lista = new Dictionary<string, Variavel>();
        }

        public void adicionar(string chave, bool usuario, Variavel var)
        {
            string nomechave;

            if (usuario)
                nomechave = "usuario." + chave;
            else
                nomechave = chave;
            Lista.Add(nomechave, var);
        }

        public bool contemVar(string chave, bool usuario)
        {
            string nomechave;

            if (usuario)
                nomechave = "usuario." + chave;
            else
                nomechave = chave;

            return Lista.ContainsKey(nomechave);
        }

        public Variaveis gerarCopia()
        {
            Variaveis copia;

            copia = new Variaveis();
            foreach(KeyValuePair<string, Variavel> entrada in Lista)
                copia.adicionar(entrada.Key, false, entrada.Value.gerarCopia());
            
            return copia;
        }

        public bool oterBooleano(string chave, bool usuario)
        {
            dynamic var;

            var = obterVar(chave, usuario);
            return (var != null) && (var.Valor == "verdadeiro");
        }

        public int obterInteiro(string chave, bool usuario)
        {
            dynamic var;

            var = obterVar(chave, usuario);
            if (var != null)
                return int.Parse(var.Valor);
            else
                return 0;
        }

        //TODO: Cada item da lista deve estar entre aspas.
        //      As aspas devem estar escapadas.
        public string[] obterLista(string chave, bool usuario)
        {
            List<string> lista;
            dynamic var;

            var = obterVar(chave, usuario);
            if (var != null)
            {
                lista = new List<string>();
                foreach (string s in var.Valor.Split(' '))
                    lista.Add(s);
                return lista.ToArray();
            }
            else
                return new string[0];
        }

        public string obterString(string chave, bool usuario)
        {
            dynamic var;

            var = obterVar(chave, usuario);
            if (var != null)
                return var;
            else
                return "";
        }

        public dynamic obterVar(string chave, bool usuario)
        {
            string nomechave;
            Variavel var;

            if (usuario)
                nomechave = "usuario." + chave;
            else
                nomechave = chave;
            Lista.TryGetValue(nomechave, out var);
            if (var != null)
                return var.Valor;
            else
                return null;
        }
    }

    public class Variavel
    {
        public bool Opcional { get; set; }

        public bool Protegida { get; set; }

        public dynamic Valor { get; set; }

        public Variavel(dynamic valor)
        {
            Valor = valor;
            Opcional = false;
            Protegida = false;
        }

        //TODO: 
        //      As aspas devem ser escapadas
        public Variavel(string[] valor)
        {
            Valor = string.Join(" ", valor);
            Opcional = false;
            Protegida = false;
        }

        public Variavel gerarCopia()
        {
            Variavel copia;

            copia = new Variavel(Valor);
            copia.Opcional = Opcional;
            copia.Protegida = Protegida;

            return copia;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('"');
            if (Valor is string && Protegida)
            {
                foreach (char _ in Valor)
                    builder.Append("■");
            }
            else
            {
                builder.Append(Valor);
            }
            builder.Append('"');

            return builder.ToString();
        }
    }
}