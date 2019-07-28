using Execucao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelagem
{
    public class Depuracao : INotifyPropertyChanged
    {
        private CentralExecucao _central;

        private StringBuilder _saida;

        //private bool iniciou;

        //private bool finalizou;

        public string Saida
        {
            get => _saida.ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Depuracao(CentralExecucao central)
        {
            _central = central;
            _saida = new StringBuilder();

            _central.CompilacaoAntes += CompilacaoIniciar;
            _central.CompilacaoDepois += CompilacaoFinalizar;

            //iniciou = false;
            //finalizou = false;
        }

        private void CompilacaoFinalizar(object sender, EventArgs e)
        {
            //iniciou = false;
            //finalizou = true;
            _saida.AppendLine(String.Format("========== Depuração finalizada: %d com êxito, %d com falha ==========", _central.TotalExitos, _central.TotalFalhas));
        }

        private void CompilacaoIniciar(object sender, EventArgs e)
        {
            //iniciou = true;
            //finalizou = false;
            _saida.AppendLine(String.Format("========== Depuração iniciada: Objeto: %s ==========", _central.ObjetoCarregado));
        }
    }
}
