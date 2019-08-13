using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace Execucao
{
    class Fluxo
    {
        private Comando _comandoatual;

        public int Id { get; private set; }

        public ObservableCollection<Comando> Instrucoes { get; private set; }

        //variáveis globais e carteira
        public Variaveis VariaveisFluxo { get; set;}

        public int Posicao { get; private set; }

        public Fluxo(int id)
        {
            Id = id;
            Posicao = 0;
            _comandoatual = null;
            Instrucoes = new ObservableCollection<Comando>();
        }

        private bool executarProximaInstrucao()
        {
            bool resultado;

            Debug.WriteLine("Executar " + _comandoatual.ToString());
            resultado = _comandoatual.executarAsync(VariaveisFluxo).Result;
            Debug.WriteLine("Comando executado.");

            return resultado;
        }

        public bool processar()
        {
            foreach (Comando c in Instrucoes)
            {
                _comandoatual = c;
                if (!executarProximaInstrucao())
                    return false;
                Posicao++;
            }

            return true;
        }

        public void adicionarComando(Comando comando)
        {
            Instrucoes.Add(comando);
        }
    }
}
