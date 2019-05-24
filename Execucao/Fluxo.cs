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

        private ObservableCollection<Variavel> _variaveis;

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

        public ObservableCollection<Variavel> Variaveis
        {
            get
            {
                return _variaveis;
            }
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
            _variaveis = new ObservableCollection<Variavel>();
        }

        public void definirEntradas(ObservableCollection<Variavel> entradas)
        {
            foreach (Variavel v in entradas)
            {
                _variaveis.Add(v);
            }
        }

        private void executarProximaInstrucao()
        {
            _comandoatual.executarAsync(Atraso);
        }

        private void processar()
        {
            foreach (Comando c in _instrucoes)
            {
                _comandoatual = c;
                executarProximaInstrucao();
                _posicao++;
                _linha++;
            }
        }
    }
}
