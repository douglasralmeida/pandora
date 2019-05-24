using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Execucao
{
    public delegate (bool, string) Funcao(Dictionary<string, dynamic> constantes, ObservableCollection<Variavel> parametros);

    public class Comando
    {
        private readonly ObservableCollection<Variavel> _parametros;

        private readonly Dictionary<string, dynamic> _constantes;

        private Funcao _funcao;

        private int _espera;

        private string _retorno;

        public Dictionary<string, dynamic> Constantes
        {
            get
            {
                return _constantes;
            }
        }

        public int Espera
        {
            get
            {
                return _espera;
            }
        }

        public string Nome
        {
            get; set;
        }

        public ObservableCollection<Variavel> Parametros
        {
            get
            {
                return _parametros;
            }
        }

        public int ParamObrigatoriosCont
        {
            get
            {
                int i = 0;
                foreach (Variavel v in _parametros)
                {
                    if (!v.Opcional)
                        i++;
                }

                return i;
            }
        }

        public string Retorno
        {
            get
            {
                return _retorno;
            }
        }

        public Comando(string nome, Funcao funcao)
        {
            Nome = nome;
            _constantes = null;
            _funcao = funcao;
            _espera = 0;
            _parametros = new ObservableCollection<Variavel>();
            _retorno = null;
        }

        public async void executarAsync(Dictionary<string, dynamic> constantes, int atraso)
        {
            int espera;
            bool executou;

            if (atraso > 0)
                espera = atraso;
            else
                espera = _espera;

            (executou, _retorno) = _funcao(constantes, _parametros);
            if (espera > 0)
                await Task.Delay(espera * 1000);
        }

        public override string ToString()
        {
            return Nome + string.Join(", ", _parametros);
        }
    }
}
