using System;
using System.Collections.Generic;
using System.Text;

namespace Execucao
{
    public delegate string Corpo(ObservableCollection<Variavel> parametros);

    class Comando
    {
        private ObservableCollection<Variavel> _parametros;

        private Corpo _corpo;

        private int _espera;

        private var _retorno;

        public string Espera
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

        public var Retorno
        {
            get
            {
                return _retorno;
            }
        }

        public string Comando(string nome, Corpo corpo)
        {
            Nome = nome;
            _corpo = corpo;
            _espera = 0;
            _parametros = new ObservableCollection<Variavel>();
            _retorno = null;
        }

        public void executar(int atraso)
        {
            int espera;

            if (atraso > 0)
                espera = atraso;
            else
                espera = _espera;

            _retorno = _corpo(_parametros);
            if (espera > 0)
                await Task.Delay(espera * 1000);
        }

        public override string ToString()
        {
            return _nome + string.Join(" ", _parametros);
        }
    }
}
