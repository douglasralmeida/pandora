using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Modelagem
{
    /// <summary>
    /// Interação lógica para TarefaControl.xaml
    /// </summary>
    public partial class TarefaView : UserControl
    {
        private readonly TarefaViewModel viewModel;

        private Base.Tarefa tarefa;

        public Base.Tarefa Tarefa
        {
            get { return tarefa; }
        }

        public TarefaView(Base.Tarefa tarefa)
        {
            InitializeComponent();
            viewModel = new TarefaViewModel(tarefa);
            DataContext = viewModel;
        }

        private void exibirTarefa()
        {
            editNome.Text = Tarefa.Nome;
            editDesc.Text = Tarefa.Descricao;
        }
    }
}
