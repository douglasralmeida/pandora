using Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private string nomeArquivo;

        Config config;

        Editor editor;

        private const string NOMEAPLICACAO = "Modelagem de Processos do Pandora";
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new NavegacaoViewModel();
            config = new Config();
            editor = new Editor();
            novoPacote();
        }

        private void BtoAbrirPacote_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogoAbrir = new OpenFileDialog();

            dialogoAbrir.Filter = "Pacote do Pandora|*.pandorapac|Demais arquivos|*.*";
            if (dialogoAbrir.ShowDialog() == true)
                abrirPacote(dialogoAbrir.FileName);
        }

        private void BtoNovoPacote_Click(object sender, RoutedEventArgs e)
        {
            novoPacote();
        }

        private void abrirPacote(string nomearquivo)
        {
            if (editor.abrir(nomearquivo))
            {
                limparUI();
                nomeArquivo = nomearquivo;
                setJanelaNome(System.IO.Path.GetFileName(nomearquivo));

                processarPacote();
                exibirPacote();
            }
        }

        private void exibirPacote()
        {
            var viewModel = (NavegacaoViewModel)DataContext;
            if (viewModel.InicioComando.CanExecute(null))
                viewModel.InicioComando.Execute(null);
        }

        private void exibirTarefa(Tarefa tarefa)
        {
            Paginas.Content = new TarefaView(tarefa);
        }

        private void limparUI()
        {
            ArvoreProcessos.Items.Clear();
            ArvoreTarefas.Items.Clear();
        }

        private void novoPacote()
        {
            limparUI();
            editor.novo(config.getNomeUsuario());
            nomeArquivo = "";
            setJanelaNome(System.IO.Path.GetFileName(editor.getNomeArquivo()));

            processarPacote();
            exibirPacote();
        }

        private void processarPacote()
        {
            processarTarefas();
            processarProcessos();
        }

        private void processarProcessos()
        {
            TreeViewItem processos;

            processos = editor.getArvoreProcessos();
            ArvoreProcessos.Items.Add(processos);
        }

        private void processarTarefas()
        {
            TreeViewItem tarefas;

            tarefas = editor.getArvoreTarefas();
            ArvoreTarefas.Items.Add(tarefas);
        }

        private void setJanelaNome(string nome)
        {
            Title = NOMEAPLICACAO + " - " + nome;
        }

        private void ArvoreProcessos_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            
        }

        private void ArvoreTarefas_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender != null)
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
                        if (treeView.SelectedItem == treeView.Items.GetItemAt(0))
                            exibirPacote();
                    }
                }
            }
        }
    }
}
