using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Execucao
{
    class Fluxo
    {
        private int _id;

        private Comando _comandoatual;

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

        public ObservableCollection<Comando> Instrucoes
        {
            get
            {
                return _instrucoes;
            }
        }

        //variáveis globais e carteira
        public Variaveis VariaveisFluxo
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
            _instrucoes = new ObservableCollection<Comando>();
        }

        private bool executarProximaInstrucao()
        {
            bool resultado;

            Debug.WriteLine("Executar " + _comandoatual.ToString());
            resultado = _comandoatual.executarAsync(VariaveisFluxo, Atraso).Result;
            Debug.WriteLine("Comando executado.");

            return resultado;
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

        public void adicionarComando(Comando comando)
        {
            _instrucoes.Add(comando);
        }
    }
}
