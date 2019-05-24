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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Depuracao(CentralExecucao central)
        {
            _central = central;
        }
    }
}
