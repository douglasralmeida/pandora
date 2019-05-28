using Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Modelagem
{
    public partial class ObjetoView : UserControl
    {
        protected Objeto _objetoativo = null;

        public event PropertyChangedEventHandler PropertyChanged;

        public Objeto ObjetoAtivo
        {
            get => _objetoativo;

            set
            {
                if (value != _objetoativo)
                {
                    _objetoativo = value;
                    OnPropertyChanged("ObjetoAtivo");
                }
            }
        }

        public ObjetoView()
        {
            DataContext = _objetoativo;
        }

        protected virtual void exibirObjeto()
        {
            throw new NotImplementedException();
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
