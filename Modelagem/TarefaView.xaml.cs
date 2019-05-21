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
        private readonly Base.Tarefa _tarefa;

        public TarefaView(Base.Tarefa tarefa)
        {
            InitializeComponent();
            _tarefa = tarefa;
            DataContext = _tarefa;
        }
        
        public bool editarOperacao(Base.Operacao operacao)
        {
            OperacaoView operacaoView = new OperacaoView(operacao);

            operacaoView.Owner = Application.Current.MainWindow;
            operacaoView.ShowDialog();

            return operacaoView.DialogResult ?? true;
        }

        public void inserirOperacao()
        {
            int quantidadeOperacoes;
            Base.Operacao novaoperacao;

            quantidadeOperacoes = _tarefa.getOperacoesCount();
            novaoperacao = new Base.Operacao(quantidadeOperacoes + 1, "NovaOperacao", "");
            if (editarOperacao(novaoperacao))
            {
                _tarefa.Operacoes.Add(novaoperacao);
            }
        }

        protected void OperacaoDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Base.Operacao operacao = ((ListViewItem)sender).Content as Base.Operacao;

            editarOperacao(operacao);
        }
    }
}
