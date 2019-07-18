using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Execucao
{
    class Fluxo
    {
        private const int EXECUCAO_PRE = 0;

        private const int EXECUCAO_NORMAL = 1;

        private const int EXECUCAO_POS = 2;

        private int _fase;

        private int _id;

        private Comando _comandoatual;

        private ObservableCollection<Fluxo> _desvios;

        private ObservableCollection<Comando>[] _instrucoes;

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

        //Entradas
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
                return _instrucoes[_fase];
            }
        }

        //variáveis globais e carteira
        public Variaveis VariaveisFluxo
        {
            get; set;
        }

        public Fluxo(int id)
        {
            _fase = 0;
            _id = id;
            Atraso = 0;
            _posicao = 0;
            _linha = 0;
            _comandoatual = null;
            _desvios = new ObservableCollection<Fluxo>();
            _instrucoes = new ObservableCollection<Comando>[3];

            //pré-execução
            _instrucoes[0] = new ObservableCollection<Comando>();
            //execução
            _instrucoes[1] = new ObservableCollection<Comando>();
            //pós-execução
            _instrucoes[2] = new ObservableCollection<Comando>();
        }

        private bool executarProximaInstrucao()
        {
            return _comandoatual.executarAsync(VariaveisFluxo, Atraso).Result;
        }

        public void processar()
        {
            while (_fase < 3)
            {
                processarFase();
                _fase++;
            }
        }

        private void processarFase()
        {
            foreach (Comando c in _instrucoes[_fase])
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

        public void adicionarComando(Comando comando, int fase)
        {
            _instrucoes[fase].Add(comando);
        }
    }
}
