using Base;
using Dialogo;
using Execucao;
using Microsoft.Win32;
using Modelagem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        Entradas _entradas;

        Dictionary<string, Variavel> _variaveis;

        private Carteira carteiraSelecionada;
        private MenuItem carteiraMenu;

        public MainWindow()
        {
            InitializeComponent();
            _editor = new Editor();
            _edicao = new EditorView(_editor);
            _editor.novo(_app.Configuracoes.UsuarioNome);
            _entradas = new Entradas();
            _variaveis = new Dictionary<string, Variavel>();
            carteiraMenu = null;
            carteiraSelecionada = null;
            DataContext = _edicao;
            ControlePrincipal.Content = _edicao;
        }

        private void addVariaveis(CentralExecucao central)
        {
            Variavel variavel;
            VarGlobais varGlobais = (Application.Current as App).VarGlobais;

            //variáveis globais
            foreach (Dado dado in varGlobais.Lista)
            {
                variavel = new Variavel(dado.Valor);
                variavel.Opcional = false;
                variavel.Protegida = false;
                central.adicionarVariaveis(dado.Nome, variavel);
            }

            //variáveis da carteira
            foreach (KeyValuePair<string, Variavel> v in _variaveis)
            {
                variavel = new Variavel(v.Value.Valor);
                variavel.Opcional = v.Value.Opcional;
                variavel.Protegida = v.Value.Protegida;
                central.adicionarVariaveis(v.Key, variavel);
            }
        }

        private void BtoAbrirPacote_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogoAbrir = new OpenFileDialog();

            dialogoAbrir.Filter = "Pacote do Pandora|*.pandorapac|Demais arquivos|*.*";
            if (checarSalvamento() && dialogoAbrir.ShowDialog() == true)
                try
                {
                    _editor.abrir(dialogoAbrir.FileName);
                }
                catch (Exception ex)
                {
                    CaixaDialogo.ErroSimples(this, ex.Message);
                }
        }

        private void BtoNovoPacote_Click(object sender, RoutedEventArgs e)
        {
            _editor.novo(_app.Configuracoes.UsuarioNome);
        }

        private void BtoSalvarPacote_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialogoSalvar = new SaveFileDialog();

            dialogoSalvar.InitialDirectory = System.IO.Path.GetDirectoryName(_editor.NomeArquivo);
            dialogoSalvar.FileName = System.IO.Path.GetFileName(_editor.NomeArquivo);
            dialogoSalvar.Filter = "Pacote do Pandora|*.pandorapac|Demais arquivos|*.*";
            if (dialogoSalvar.ShowDialog() == true)
                _editor.salvar(dialogoSalvar.FileName);
        }

        private void BtoDepurar_Click(object sender, RoutedEventArgs e)
        {
            CentralExecucao central = new CentralExecucao();
            Thread t = new Thread(central.processar);
            t.Name = "Pandora_Execucao";

            central.Erros = _editor.Erros;
            _depurador = new Depuracao(central);
            _depuracao = new DepuracaoView(_depurador);
            _editor.limparErros();
            try
            {
                Mouse.OverrideCursor = Cursors.AppStarting;
                ControlePrincipal.Content = _depuracao;
                central.gerarInstancia();
                central.definirEntradas(_entradas);
                addVariaveis(central);
                central.carregar(_edicao.ObjetoAtivo);
                if (checarCarteira())
                {
                    central.preparar();
                    // chama central.processar() em uma thread separada
                    t.SetApartmentState(ApartmentState.STA);
                    t.IsBackground = true;
                    t.Start();
                    Thread.Sleep(1000);
                }
                else
                {
                    //Gerar erro: Nenhuma carteira aberta.
                    central.Erros.Adicionar("CT0001", new string[0]);
                }
            }
            finally
            {
                if (t.IsAlive)
                    t.Join();
                ControlePrincipal.Content = _edicao;
                Mouse.OverrideCursor = null;
            }
        }

        private void BtoOpcoesEntrada_Click(object sender, RoutedEventArgs e)
        {
            string[] cabecalho;
            string[][] entradas;
            EntradasView entradasVisao;
            Objeto objeto = _edicao.ObjetoAtivo;

            cabecalho = objeto.obterEntradas();
            entradas = _entradas.ObterDados(cabecalho);
            entradasVisao = new EntradasView(cabecalho, entradas);
            entradasVisao.Owner = Application.Current.MainWindow;
            entradasVisao.ShowDialog();
            if (entradasVisao.DialogResult ?? true)
            {
                _entradas.Limpar();
                _entradas.DefinirCabecalho(cabecalho);
                foreach (DataRow linha in entradasVisao.Dados.Rows)
                {
                    Execucao.Entrada ent = new Execucao.Entrada();
                    foreach (DataColumn coluna in entradasVisao.Dados.Columns)
                    {
                        ent.AdicionarVariavel(coluna.Caption, linha[coluna].ToString());
                    }
                    if (!ent.TudoVazio)
                        _entradas.Adicionar(ent);
                }              
                //_app.Configuracoes.setEntradasFromString(entradasVisao.Entradas);
                //_app.Configuracoes.DirTrabalho = entradasVisao.Dir;
            }
        }

        private void BtoVariaveisGlobais_Click(object sender, RoutedEventArgs e)
        {
            VariaveisGlobaisView varGlobais = new VariaveisGlobaisView();

            varGlobais.Owner = Application.Current.MainWindow;
            varGlobais.abrirVar();
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
                if (carteiraMenu != null)
                    Carteira_OnClick(carteiraMenu, new RoutedEventArgs());
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
            carteiraMenu = menu;
            sd.ShowDialog();
            if (sd.DialogResult ?? true)
            {
                menu.IsChecked = true;
            }
            else
            {
                carteiraSelecionada = null;
                menu.IsChecked = false;
                _variaveis.Clear();
                CaixaDialogo.ErroSimples(this, SENHA_ERRADA);
            }
        }

        private bool checarCarteira()
        {
            return (carteiraSelecionada != null);
        }

        private bool checarSalvamento()
        {
            const string PERGUNTA_SALVAR = "Você deseja salvar as alterações feitas no pacote atual?";

            if (_editor.Modificado)
            {
                if (CaixaDialogo.PerguntaSimples(this, PERGUNTA_SALVAR))
                {
                    BtoSalvarPacote_Click(null, null);
                    return !_editor.Modificado;
                }
                else
                    return true;
            }
            else
                return true;
        }

        void JanelaPadrao_Fechando(object sender, CancelEventArgs e)
        {
            if (!checarSalvamento())
                e.Cancel = true;
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
                if (!c.Value.individual)
                    continue;
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
