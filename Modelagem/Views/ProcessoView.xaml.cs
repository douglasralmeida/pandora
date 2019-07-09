using Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interação lógica para ProcessoControl.xam
    /// </summary>
    public partial class ProcessoView : ObjetoView
    {
        private Processo _processoativo => (Processo)_objetoativo;

        public ProcessoView()
        {
            InitializeComponent();
            processarAtividades();
        }

        private void processarAtividades()
        {
            Processo processoativo = ObjetoAtivo as Processo;

            ICollectionView view = CollectionViewSource.GetDefaultView(processoativo.Atividades.Pre);
            view.GroupDescriptions.Add(new PropertyGroupDescription("Fase"));
            view.SortDescriptions.Add(new SortDescription("Nome", ListSortDirection.Ascending));
            ListaAtividades.ItemsSource = view;
        }

        protected void AtividadeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Objeto objeto = ((ListViewItem)sender).Content as Objeto;

            ObjetoAtivo = objeto;
        }

        private void BtoInserirAtividade_Click(object sender, RoutedEventArgs e)
        {
            Objeto objeto;

            AtividadesView atividadesView = new AtividadesView()
            {
                Owner = Application.Current.MainWindow
            };
            atividadesView.TodasAtividades_Adicionar(TodasTarefas);
            atividadesView.TodasAtividades_Adicionar(TodosProcessos);
            atividadesView.ShowDialog();

            if (atividadesView.DialogResult ?? true)
            {
                objeto = atividadesView.ObjetoSelecionado;
                _processoativo.Atividades.Add(objeto);
            }

        }

        private void BtoExcluirAtividade_Click(object sender, RoutedEventArgs e)
        {
            const string PERG_EXCLUIRAT_1 = "Você tem certeza que deseja excluir a atividade selecionada do processo atual?";
            const string PERG_EXCLUIRAT_MAIS1 = "Você tem certeza que deseja excluir todas as atividades selecionadas do processo atual?";
            string mensagem;

            if (ListaAtividades.SelectedItems.Count == 1)
                mensagem = PERG_EXCLUIRAT_1;
            else
                mensagem = PERG_EXCLUIRAT_MAIS1;

            if (CaixaDialogo.PerguntaSimples(mensagem))
            {
                if (ListaAtividades.SelectedItems.Count > 0)
                {
                    var lista = ListaAtividades.SelectedItems.Cast<Objeto>().ToList();
                    foreach (Objeto objeto in lista)
                    {
                        _processoativo.Atividades.Remove(objeto);
                    }
                }
            }
        }
    }
}
