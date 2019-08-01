using Base;
using Modelagem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Execucao
{
    public class Parametros : IEnumerable<string>
    {
        private ObservableCollection<Variavel> lista;

        public int Cont { get => lista.Count; }

        public int ObrigatoriosCont
        {
            get
            {
                int i = 0;
                foreach (Variavel v in lista)
                {
                    if (!v.Opcional)
                        i++;
                }

                return i;
            }
        }

        public Parametros()
        {
            lista = new ObservableCollection<Variavel>();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach(Variavel v in lista)
            {
                sb.Append('"');
                sb.Append(v.Valor);
                sb.Append('"');
                sb.Append(' ');
            }
            if (sb.Length > 2)
                sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }

        public IEnumerator<string> GetEnumerator()
        {
            List<string> valores = new List<string>();

            foreach (Variavel v in lista)
                valores.Add(v.Valor);

            return valores.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Add(Variavel variavel)
        {
            lista.Add(variavel);
        }
    }

    /* bool -> se funcao executou corretamente
     * string -> parametros da funcao
     */
    public delegate (bool, string) Funcao(Variaveis variaveis, Parametros parametros);

    public class Comando
    {
        private Funcao _funcao;

        public Variaveis Dados { get; set; }

        public int Espera { get; private set; }

        public string Nome
        {
            get; set;
        }

        public Parametros ListaParametros { get; }

        public int ParamObrigatorios => ListaParametros.ObrigatoriosCont;

        public string Retorno { get; private set; }

        public Comando(string nome, Funcao funcao)
        {
            App app = (Application.Current as App);
            Config config = app.Configuracoes;

            Nome = nome;
            Dados = null;
            _funcao = funcao;
            Espera = config.Intervalo;
            ListaParametros = new Parametros();
            Retorno = null;
        }

        public async Task<bool> executarAsync(Variaveis variaveis, int atraso)
        {
            int espera;
            bool executou;

            if (atraso > 0)
                espera = atraso;
            else
                espera = Espera;

            //constantes = variáveis globais e carteira
            //Parametros = argumentos da execução
            (executou, Retorno) = _funcao(variaveis, ListaParametros);
            if (espera > 0)
                await Task.Delay(espera);

            return executou;
        }

        public override string ToString()
        {
            return Nome + " " + string.Join(", ", ListaParametros);
        }

        internal void AddParametro(Variavel variavel)
        {
            ListaParametros.Add(variavel);
        }
    }
}