using Dialogo;
using Modelagem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace Modelagem
{
    /// <summary>
    /// Lógica interna para CarteirasView.xaml
    /// </summary>
    public partial class CarteirasView : Window
    {
        CarteiraView visaoCarteira;

        private Carteira carteira = null;

        public ObservableCollection<Carteira> Carteiras { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CarteirasView()
        {
            InitializeComponent();
            Carteiras = (Application.Current as App).Carteiras.Lista;
            DataContext = this;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            irParaPaginaInicial();
        }

        private void BtoNovaCarteira_Click(object sender, RoutedEventArgs e)
        {
            carteira = new Carteira();
            visaoCarteira = new CarteiraView(carteira);
            visaoCarteira.PropertyChanged += VisaoCarteira_PropertyChanged;
            visaoCarteira.criarCarteira();
            Pagina.Content = visaoCarteira;
        }

        private bool ExibirCarteira(byte[] hash)
        {
            visaoCarteira = new CarteiraView(carteira);
            visaoCarteira.PropertyChanged += VisaoCarteira_PropertyChanged;            
            bool resultado = visaoCarteira.abrirCarteira(hash);
            if (resultado)
                Pagina.Content = visaoCarteira;

            return resultado;
        }

        private void ListaCarteiras_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ListaCarteiras.SelectedItem != null)
            {
                carteira = (ListaCarteiras.SelectedItem as Carteira);
                SolicitarSenha();
            }
            else
            {
                carteira = null;
                irParaPaginaInicial();
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void SolicitarSenha()
        {
            SenhaDialogo sd;

            SenhaDialogo.ChecarSenhaProc csproc = delegate (byte[] hash)
            {
                return ExibirCarteira(hash);
            };
            sd = new SenhaDialogo()
            {
                Owner = Application.Current.MainWindow,
                checarSenha = csproc
            };
            sd.ShowDialog();
            if (!sd.DialogResult ?? true)
            {
                ListaCarteiras.SelectedItem = null;
            }
        }

        private void VisaoCarteira_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        private void irParaPaginaInicial()
        {
            Pagina.Content = new CarteiraInicio();
        }
    }
}
