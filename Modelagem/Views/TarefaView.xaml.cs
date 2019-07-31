using Base;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public bool editarOperacao(Operacao operacao)
        {
            OperacaoView operacaoView = new OperacaoView(operacao)
            {
                Owner = Application.Current.MainWindow,
                ModuloUtilizado = _tarefaativa.Modulo
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
            Operacao novaoperacao;

            quantidadeOperacoes = _tarefaativa.getOperacoesCount();
            novaoperacao = new Operacao(quantidadeOperacoes + 1, "NovaOperacao", new string[0]);
            if (editarOperacao(novaoperacao))
            {
                _tarefaativa.Operacoes.Add(novaoperacao);
            }
        }

        protected void OperacaoDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Operacao operacao = ((ListViewItem)sender).Content as Base.Operacao;

            editarOperacao(operacao);
        }

        private void BtoExcluirOperacao_Click(object sender, RoutedEventArgs e)
        {
            const string PERG_EXCLUIROP_1 = "Você tem certeza que deseja excluir a operação selecionada?";
            const string PERG_EXCLUIROP_MAIS1 = "Você tem certeza que deseja excluir todas as operações selecionadas?";
            string mensagem;

            if (listaOperacoes.SelectedItems.Count == 1)
                mensagem = PERG_EXCLUIROP_1;
            else
                mensagem = PERG_EXCLUIROP_MAIS1;

            if (CaixaDialogo.PerguntaSimples(this, mensagem))
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
