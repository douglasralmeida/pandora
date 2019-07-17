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

        private ICollectionView _visao;

        public ICollectionView Atividades
        {
            get
            {
                return _visao;
            }
        }
        

        public ProcessoView()
        {
            InitializeComponent();

            //PropertyChanged += ProcessoView_PropertyChanged;
        }

        private void ProcessoView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ObjetoAtivo")
                processarAtividades();
        }

        private void processarAtividades()
        {
            Processo processoativo = _processoativo;

            if (processoativo == null)
                return;

            _visao = CollectionViewSource.GetDefaultView(_processoativo.Atividades);
            _visao.GroupDescriptions.Add(new PropertyGroupDescription("Fase"));
            _visao.SortDescriptions.Add(new SortDescription("Nome", ListSortDirection.Ascending));
        }

        protected void AtividadeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Atividade atividade = ((ListViewItem)sender).Content as Atividade;
            ObjetoAtivo = atividade.ObjetoRelacionado;
        }

        private void BtoInserirAtividade_Click(object sender, RoutedEventArgs e)
        {
            Atividade atividade;
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
                atividade = new Atividade(objeto);
                atividade.Fase = atividadesView.FaseEscolhida;
                _processoativo.adicionarAtividade(atividade);
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
                    var lista = ListaAtividades.SelectedItems.Cast<Atividade>().ToList();
                    foreach (Atividade atividade in lista)
                    {
                        _processoativo.Atividades.Remove(atividade);
                    }
                }
            }
        }
    }
}
