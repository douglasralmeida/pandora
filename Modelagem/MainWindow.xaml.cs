using Base;
using Execucao;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace Modelagem
{
    public class BoolToStringConverter : BoolToValueConverter<string> { };

    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        Config _config;

        DepuracaoView _depuracao;

        Depuracao _depurador;

        EditorView _edicao;

        Editor _editor;

        private const string NOMEAPLICACAO = "Modelagem de Processos do Pandora";
        public MainWindow()
        {
            InitializeComponent();
            _config = new Config();
            _editor = new Editor();
            _edicao = new EditorView(_editor);
            _editor.novo(_config.UsuarioNome);
            DataContext = _edicao;
            ControlePrincipal.Content = _edicao;
        }

        private void BtoAbrirPacote_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogoAbrir = new OpenFileDialog();

            dialogoAbrir.Filter = "Pacote do Pandora|*.pandorapac|Demais arquivos|*.*";
            if (dialogoAbrir.ShowDialog() == true)
                _editor.abrir(dialogoAbrir.FileName);
        }

        private void BtoNovoPacote_Click(object sender, RoutedEventArgs e)
        {
            _editor.novo(_config.UsuarioNome);
        }

        private void BtoSalvarPacote_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialogoSalvar = new SaveFileDialog();

            dialogoSalvar.FileName = _editor.NomeArquivo;
            dialogoSalvar.Filter = "Pacote do Pandora|*.pandorapac|Demais arquivos|*.*";
            if (dialogoSalvar.ShowDialog() == true)
                _editor.salvar(dialogoSalvar.FileName);
        }

        private void BtoDepurar_Click(object sender, RoutedEventArgs e)
        {
            CentralExecucao central = new CentralExecucao();
            Thread t = new Thread(central.processar);

            _depurador = new Depuracao(central);
            _depuracao = new DepuracaoView(_depurador);
            try
            {
                Mouse.OverrideCursor = Cursors.AppStarting;
                ControlePrincipal.Content = _depuracao;
                central.gerarInstancia();
                central.Variaveis = _config.Entradas;
                central.carregar(_edicao.ObjetoAtivo);
                // chama central.processar() em uma thread separada
                t.Start();
                Thread.Sleep(5000);
            }
            finally
            {
                t.Join();
                ControlePrincipal.Content = _edicao;
                Mouse.OverrideCursor = null;
            }
        }

        private void BtoOpcoesEntrada_Click(object sender, RoutedEventArgs e)
        {
            EntradasView entradasVisao = new EntradasView(_config.Entradas.ToString(), _config.DirTrabalho);

            entradasVisao.Owner = Application.Current.MainWindow;
            entradasVisao.ShowDialog();

            if (entradasVisao.DialogResult ?? true)
            {
                _config.setEntradasFromString(entradasVisao.Entradas);
                _config.DirTrabalho = entradasVisao.Dir;
            }
        }
    }
}
