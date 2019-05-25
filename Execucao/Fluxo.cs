using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Execucao
{
    class Fluxo
    {
        private int _id;

        private Comando _comandoatual;

        private ObservableCollection<Fluxo> _desvios;

        private ObservableCollection<Comando> _instrucoes;

        private int _posicao;

        private int _linha;

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public int Atraso
        {
            get; set;
        }

        public int Linha
        {
            get
            {
                return _linha;
            }
        }

        public Dictionary<string, dynamic> Dados
        {
            get; set;
        }

        public ObservableCollection<Fluxo> Desvios
        {
            get
            {
                return _desvios;
            }
        }

        public ObservableCollection<Comando> Instrucoes
        {
            get
            {
                return _instrucoes;
            }
        }

        public Dictionary<string, Variavel> Variaveis
        {
            get; set;
        }

        public Fluxo(int id)
        {
            _id = id;
            Atraso = 0;
            _posicao = 0;
            _linha = 0;
            _comandoatual = null;
            _desvios = new ObservableCollection<Fluxo>();
            _instrucoes = new ObservableCollection<Comando>();
        }

        private bool executarProximaInstrucao()
        {
            return _comandoatual.executarAsync(Dados, Atraso).Result;
        }

        public void processar()
        {
            foreach (Comando c in _instrucoes)
            {
                _comandoatual = c;
                if (!executarProximaInstrucao())
                {
                    break;
                }
                _posicao++;
                _linha++;
            }
        }
    }
}
