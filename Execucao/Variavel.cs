﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Execucao
{
    public class Variaveis
    {
        public Dictionary<string, Variavel> Lista { get; private set; }

        public Variaveis()
        {
            Lista = new Dictionary<string, Variavel>();
        }

        public void adicionar(string nome, Variavel var)
        {
            Lista.Add(nome, var);
        }

        public bool contemVar(string chave)
        {
            return Lista.ContainsKey(chave);
        }

        public dynamic obterVar(string chave)
        {
            Variavel var;

            Lista.TryGetValue(chave, out var);
            if (var != null)
                return var.Valor;
            else
                return null;
        }
    }

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

        public dynamic Valor
        {
            get; set;
        }

        public Variavel(dynamic valor)
        {
            Valor = valor;
            Opcional = false;
            Protegida = false;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            builder.Append('"');
            if (Valor is string && Protegida)
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