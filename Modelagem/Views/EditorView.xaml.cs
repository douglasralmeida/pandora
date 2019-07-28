using Base;
using Modelagem.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Modelagem
{
    /// <summary>
    /// Interação lógica para EditorView.xam
    /// </summary>
    public partial class EditorView : UserControl, INotifyPropertyChanged
    {
        private const string PROCESSO_UTILIZADO = "O processo selecionado é utilizado como atividade em outros processos. Ao excluí-lo, essas atividades também serão exclúidas. Você deseja excluí-lo mesmo assim?";

        private const string TAREFA_UTILIZADA = "A tarefa selecionada é utilizada como atividade em outros processos. Ao excluí-la, essas atividades também serão exclúidas. Você deseja excluí-la mesmo assim?";

        private Editor _editor;

        private Objeto _objetoativo;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Carteira> ListaCarteiras
        {
            get => (Application.Current as App).Carteiras.Lista;
        }

        public bool Modificado
        {
            get => _editor.Modificado;
        }

        public string NomeArquivo
        {
            get => _editor.NomeArquivo;
        }

        public Objeto ObjetoAtivo
        {
            get
            {
                return _objetoativo;
            }

            set
            {
                if (_objetoativo != value)
                {
                    _objetoativo = value;
                    OnPropertyChanged("ObjetoAtivo");
                    OnPropertyChanged("PodeDepurar");
                }
            }
        }

        public bool PodeDepurar
        {
            get => (ObjetoAtivo != null);
        }

        public EditorView(Editor editor)
        {
            InitializeComponent();
            _editor = editor;
            _editor.TarefaAdded += Editor_TarefaAdded;
            _editor.PropertyChanged += Editor_PropertyChanged;
            _objetoativo = null;
            DataContext = _editor;
        }

        private void ArvoreProcessos_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(Paginas.Content is ProcessoView))
                ArvoreProcessos_SelectedItemChanged(sender, null);
        }

        public void ArvoreProcessos_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            var processo = treeView.SelectedItem as Processo;
            if (processo != null)
            {
                exibirProcesso(processo);
            }
            else
            {
                if (treeView.SelectedItem != null && treeView.SelectedItem == treeView.Items.GetItemAt(0))
                    exibirTodosProcessos();
            }
        }

        private void ArvoreTarefas_GotFocus(object sender, RoutedEventArgs e)
        {
            if (!(Paginas.Content is TarefaView))
                ArvoreTarefas_SelectedItemChanged(sender, null);
        }

        public void ArvoreTarefas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var treeView = sender as TreeView;
            if (treeView != null)
            {
                var tarefa = treeView.SelectedItem as Tarefa;
                if (tarefa != null)
                {
                    exibirTarefa(tarefa);
                }
                else
                {
                    if (treeView.SelectedItem != null && treeView.SelectedItem == treeView.Items.GetItemAt(0))
                        exibirTodasTarefas();
                }
            }
        }

        private void BtoInserirProcesso_Click(object sender, RoutedEventArgs e)
        {
            _editor.inserirProcesso();
        }

        private void BtoInserirTarefa_Click(object sender, RoutedEventArgs e)
        {
            _editor.inserirTarefa();
        }

        private void BtoExcluirProcesso_Click(object sender, RoutedEventArgs e)
        {
            var processo = ArvoreProcessos.SelectedItem as Processo;

            if (processo != null)
            {
                if (_editor.objetoUtilizadoComoAtividade(processo))
                    if (!CaixaDialogo.PerguntaSimples(this, PROCESSO_UTILIZADO))
                        return;
                exibirTodosProcessos();
                _editor.excluirProcesso(processo);
            }
        }

        private void BtoExcluirTarefa_Click(object sender, RoutedEventArgs e)
        {
            var tarefa = ArvoreTarefas.SelectedItem as Tarefa;

            if (tarefa != null)
            {
                if (_editor.objetoUtilizadoComoAtividade(tarefa))
                    if (!CaixaDialogo.PerguntaSimples(this, TAREFA_UTILIZADA))
                        return;
                exibirTodasTarefas();
                _editor.excluirTarefa(tarefa);
            }
        }

        private void Editor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string[] propriedades = { "Modificado", "NomeArquivo" };

            if (propriedades.Contains(e.PropertyName))
                OnPropertyChanged(e.PropertyName);
        }

        public void Editor_TarefaAdded(object sender, Tarefa tarefa)
        {
            var item = ArvoreTarefas.Items.GetItemAt(0) as TreeViewItem;

            if (item != null)
            {
                var filho = item.ItemContainerGenerator.ContainerFromItem(tarefa) as TreeViewItem;
                if (filho != null)
                {
                    filho.IsSelected = true;
                }
            }
        }

        private void exibirProcesso(Processo processo)
        {
            ProcessoView processoview = new ProcessoView();

            processoview.TodosProcessos = _editor.Processos;
            processoview.TodasTarefas = _editor.Tarefas;
            processoview.PropertyChanged += ObjetoView_PropertyChanged;
            processoview.ObjetoAtivo = processo;
            Paginas.Content = processoview;
//            ObjetoAtivo = processo;
        }

        public void exibirTarefa(Tarefa tarefa)
        {
            TarefaView tarefaview = new TarefaView();

            tarefaview.PropertyChanged += ObjetoView_PropertyChanged;
            tarefaview.ObjetoAtivo = tarefa;
            Paginas.Content = tarefaview;
//            ObjetoAtivo = tarefa;
        }

        public void exibirTodosProcessos()
        {
            Paginas.Content = new TodosProcessosView();
            ObjetoAtivo = null;
        }

        public void exibirTodasTarefas()
        {
            Paginas.Content = new TodasTarefasView();
            ObjetoAtivo = null;
        }

        private void ObjetoView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ObjetoAtivo")
            {
                var prop = sender.GetType().GetProperty(e.PropertyName);
                ObjetoAtivo = prop == null ? null : prop.GetValue(sender, null) as Objeto;
            }
            else if (e.PropertyName == "TipoObjeto")
            {
                var prop = sender.GetType().GetProperty("ObjetoAtivo");
                Objeto objeto = prop == null ? null : prop.GetValue(sender, null) as Objeto;

                if (objeto is Tarefa)
                    exibirTarefa((Tarefa)objeto);
                else if (objeto is Processo)
                    exibirProcesso((Processo)objeto);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}