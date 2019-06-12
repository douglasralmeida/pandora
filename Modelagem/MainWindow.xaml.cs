using Base;
using Dialogo;
using Execucao;
using Microsoft.Win32;
using Modelagem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Modelagem
{
    public class BoolToStringConverter : BoolToValueConverter<string> { };

    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string SENHA_ERRADA = "A senha informada está incorreta. Tente novamente.";

        App _app = (Application.Current as App);

        DepuracaoView _depuracao;

        Depuracao _depurador;

        EditorView _edicao;

        Editor _editor;

        Dictionary<string, Variavel> _variaveis;

        private Carteira carteiraSelecionada;

        public MainWindow()
        {
            InitializeComponent();
            _editor = new Editor();
            _edicao = new EditorView(_editor);
            _editor.novo(_app.Configuracoes.UsuarioNome);
            _variaveis = new Dictionary<string, Variavel>();
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
            _editor.novo(_app.Configuracoes.UsuarioNome);
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
                //central.adicionarVariaveis(s, v);
                //central.Variaveis = _app.Configuracoes.Entradas;
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
            EntradasView entradasVisao = new EntradasView(_app.Configuracoes.Entradas.ToString(), _app.Configuracoes.DirTrabalho); ;

            entradasVisao.Owner = Application.Current.MainWindow;
            entradasVisao.ShowDialog();

            if (entradasVisao.DialogResult ?? true)
            {
                _app.Configuracoes.setEntradasFromString(entradasVisao.Entradas);
                _app.Configuracoes.DirTrabalho = entradasVisao.Dir;
            }
        }

        private void BtoVariaveisGlobais_Click(object sender, RoutedEventArgs e)
        {
            VariaveisGlobaisView varGlobais = new VariaveisGlobaisView();

            varGlobais.Owner = Application.Current.MainWindow;
            varGlobais.ShowDialog();

            if (varGlobais.DialogResult ?? true)
            {

            }
        }

        private void BtoCarteiras_Click(object sender, RoutedEventArgs e)
        {
            CarteirasView carteirasVisao = new CarteirasView();

            carteirasVisao.Owner = Application.Current.MainWindow;
            carteirasVisao.ShowDialog();

            if (carteirasVisao.DialogResult ?? true)
            {

            }
        }

        private void Carteira_OnClick(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (sender as MenuItem);
            SenhaDialogo sd;

            SenhaDialogo.ChecarSenhaProc csproc = delegate (byte[] hash)
            {
                return selecionarCarteira(hash);
            };
            sd = new SenhaDialogo()
            {
                Owner = Application.Current.MainWindow,
                checarSenha = csproc
            };
            carteiraSelecionada = (menu.CommandParameter as Carteira);
            sd.ShowDialog();
            if (sd.DialogResult ?? true)
            {
                menu.IsChecked = true;
            }
            else
            {
                menu.IsChecked = false;
                CaixaDialogo.ErroSimples(SENHA_ERRADA);
            }
        }

        private bool selecionarCarteira(byte[] hash)
        {
            string valor;
            KeyValuePair<string, ConstanteInfo>[] ctes = BibliotecaPadrao.Biblioteca.obterCtesChaves();

            _variaveis.Clear();

            valor = carteiraSelecionada.obterItem("PALAVRA_MAGICA", hash);
            if (valor != "!abracadabra1")
                return false;
            foreach (KeyValuePair<string, ConstanteInfo> c in ctes)
            {
                if (carteiraSelecionada.Dados.ContainsKey(c.Key))
                {
                    valor = carteiraSelecionada.obterItem(c.Key, hash);
                }
                else
                {
                    valor = "";
                }
                Variavel var = new Variavel(valor);
                var.Opcional = false;
                var.Protegida = c.Value.oculta;
                _variaveis.Add(c.Key, var);
            }

            return true;
        }
    }
}
