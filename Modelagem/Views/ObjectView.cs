using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ObservableCollection<Processo> TodosProcessos;

        public ObservableCollection<Tarefa> TodasTarefas;

        public event PropertyChangedEventHandler PropertyChanged;

        public Objeto ObjetoAtivo
        {
            get => _objetoativo;

            set
            {
                if (value != _objetoativo)
                {
                    if (_objetoativo != null && _objetoativo.GetType() != value.GetType())
                    {
                        _objetoativo = value;
                        OnPropertyChanged("TipoObjeto");
                    }
                    else
                    {
                        _objetoativo = value;
                        DataContext = _objetoativo;
                        OnPropertyChanged("ObjetoAtivo");
                    }
                }
            }
        }

        public ObjetoView()
        {
            
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
