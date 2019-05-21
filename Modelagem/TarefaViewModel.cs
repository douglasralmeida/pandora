using Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Modelagem
{
    public class TarefaViewModel : INotifyPropertyChanged
    {
        private Base.Tarefa tarefa;

        public TarefaViewModel(Base.Tarefa tarefa)
        {
            this.tarefa = tarefa;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public string Nome
        {
            get { return tarefa.Nome; }
            set
            {
                if (tarefa.Nome != value)
                {
                    tarefa.Nome = value;
                    OnPropertyChange("Nome");
                }
            }
        }

        public string Descricao
        {
            get { return tarefa.Descricao; }
            set
            {
                if (tarefa.Descricao != value)
                {
                    tarefa.Descricao = value;
                    OnPropertyChange("Descricao");
                }
            }
        }

        public ObservableCollection<Operacao> Operacoes
        {
            get { return tarefa.Operacoes; }
        }
    }
}
