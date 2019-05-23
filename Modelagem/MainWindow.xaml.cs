using Base;
using Execucao;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Modelagem
{
    public class BoolToStringConverter : BoolToValueConverter<String> { };

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

        List<Variavel> _entradas;

        private const string NOMEAPLICACAO = "Modelagem de Processos do Pandora";
        public MainWindow()
        {
            InitializeComponent();
            _config = new Config();
            _editor = new Editor();
            _edicao = new EditorView(_editor);
            _editor.novo(_config.getNomeUsuario());
            _entradas = new List<Variavel>();
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
            _editor.novo(_config.getNomeUsuario());
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

            central.carregarEntradas(entradas);
            central.carregar(_edicao.ObjetoAtivo);

            _depurador = new Depuracao(central);
            _depuracao = new DepuracaoView(_depurador);
            try
            {
                ControlePrincipal.Content = _depurador;
                t.Start();
                Thread.Sleep(5000);
            }
            finally
            {
                t.Join();
                ControlePrincipal.Content = _edicao;
            }
        }

        private void BtoOpcoesEntrada_Click(object sender, RoutedEventArgs e)
        {
            EntradasView entradasVisao = new EntradasView();

            entradasVisao.Owner = Application.Current.MainWindow;
            entradasVisao.Entradas = Config.getEntradasToString();
            entradasVisao.Dir = Config.DirTrabalho;
            entradasVisao.ShowDialog();

            if (entradasVisao.DialogResult ?? true)
            {
                Config.setEntradasFromString(entradasVisao.Entradas);
                Config.DirTrabalho = entradasVisao.Dir;
            }
        }
    }
}
