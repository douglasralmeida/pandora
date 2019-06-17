using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Modelagem
{
    public class Dado
    {
        public string Nome { get; set; }

        public string Valor { get; set; }

        public Dado()
        {
            Nome = "";
            Valor = "";
        }

        public Dado(string nome, string valor)
        {
            Nome = nome;
            Valor = valor;
        }
    }

    public class VarGlobais
    {
        private const string ARQ_VARGLOBAIS= "varglobais.json";

        private Arquivo _arquivo;

        public ObservableCollection<Dado> Lista { get; private set; } = null;

        public VarGlobais()
        {
            _arquivo = (Application.Current as App).Arquivo;
        }

        public void alterarVariavel(string variavel, string valor)
        {
            foreach(Dado d in Lista)
            {
                if (d.Nome == variavel)
                {
                    d.Valor = valor;
                    return;
                }
            }
            Lista.Add(new Dado(variavel, valor));
        }

        public void Carregar()
        {
            Lista = _arquivo.processarLista<Dado>(ARQ_VARGLOBAIS);
        }

        public void Salvar()
        {
            _arquivo.salvarLista(Lista, ARQ_VARGLOBAIS);
        }

        public bool contemVariavel(string variavel)
        {
            foreach (Dado d in Lista)
            {
                if (d.Nome == variavel)
                return true;
            }

            return false;
        }

        public string obterVariavel(string variavel)
        {
            foreach (Dado d in Lista)
            {
                if (d.Nome == variavel)
                    return d.Valor;
            }

            return "";
        }
    }
}
