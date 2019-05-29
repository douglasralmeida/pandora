using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class TarefaView : ObjetoView
    {

        private Tarefa _tarefaativa
        {
            get => (Tarefa)_objetoativo;
        }


        public TarefaView()
        {
            InitializeComponent();
        }

        public bool editarOperacao(Base.Operacao operacao)
        {
            OperacaoView operacaoView = new OperacaoView(operacao)
            {
                Owner = Application.Current.MainWindow
            };
            operacaoView.ShowDialog();

            return operacaoView.DialogResult ?? true;
        }

        public void excluirOperacao(Base.Operacao operacao)
        {
            _tarefaativa.Operacoes.Remove(operacao);
        }

        public void inserirOperacao()
        {
            int quantidadeOperacoes;
            Base.Operacao novaoperacao;

            quantidadeOperacoes = _tarefaativa.getOperacoesCount();
            novaoperacao = new Base.Operacao(quantidadeOperacoes + 1, "NovaOperacao", "");
            if (editarOperacao(novaoperacao))
            {
                _tarefaativa.Operacoes.Add(novaoperacao);
            }
        }

        protected void OperacaoDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Base.Operacao operacao = ((ListViewItem)sender).Content as Base.Operacao;

            editarOperacao(operacao);
        }

        private void BtoExcluirOperacao_Click(object sender, RoutedEventArgs e)
        {
            const string PERG_EXCLUIROP = "Você tem certeza que deseja excluir a operação selecionada?";

            if (CaixaDialogo.PerguntaSimples(PERG_EXCLUIROP))
            {
                if (listaOperacoes.SelectedItems.Count > 0)
                {
                    var lista = listaOperacoes.SelectedItems.Cast<Base.Operacao>().ToList();
                    foreach (Base.Operacao operacao in lista)
                    {
                        excluirOperacao(operacao);
                    }
                    _tarefaativa.reprocessarOperacaoIds();
                }
            }
        }

        private void BtoIncluirOperacao_Click(object sender, RoutedEventArgs e)
        {
            inserirOperacao();
        }
    }
}
