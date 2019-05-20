using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Modelagem
{
    class NavegacaoViewModel: INotifyPropertyChanged
    {
        public ICommand TarefaComando { get; set; }
        public ICommand InicioComando { get; set; }

        private object escolhidoViewModel;

        public object EscolhidoViewModel
        {
            get { return escolhidoViewModel; }
            set { escolhidoViewModel = value; OnPropertyChanged("EscolhidoViewModel"); }
        }


        public NavegacaoViewModel()
        {
            TarefaComando = new BaseComando(AbrirTarefa);
            InicioComando = new BaseComando(AbrirInicio);
        }

        private void AbrirTarefa(object obj)
        {
            EscolhidoViewModel = new TarefaViewModel(obj);
        }
        private void AbrirInicio(object obj)
        {
            EscolhidoViewModel = new InicioViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

    public class BaseComando : ICommand
    {
        private Predicate<object> _canExecute;
        private Action<object> _method;
        public event EventHandler CanExecuteChanged;

        public BaseComando(Action<object> method) : this(method, null)
        {

        }

        public BaseComando(Action<object> method, Predicate<object> canExecute)
        {
            _method = method;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }

            return _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _method.Invoke(parameter);
        }
    }
}