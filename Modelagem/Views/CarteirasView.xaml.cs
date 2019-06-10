using Modelagem.Views;
using System;
using System.Collections.Generic;
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

        public List<Carteira> Carteiras { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public CarteirasView(List<Carteira> carteiras)
        {
            InitializeComponent();
            Carteiras = carteiras;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            irParaPaginaInicial();
        }

        private void BtoNovaCarteira_Click(object sender, RoutedEventArgs e)
        {
            carteira = new Carteira();
            ExibirCarteira(new byte[1]);
        }

        private void ExibirCarteira(byte[] hash)
        {
            visaoCarteira = new CarteiraView(carteira);
            visaoCarteira.PropertyChanged += VisaoCarteira_PropertyChanged;
            Pagina.Content = visaoCarteira;
            visaoCarteira.abrirCarteira(hash);
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
            byte[] hash;

            hash = new byte[1];

            ExibirCarteira(hash);
        }

        private void VisaoCarteira_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Exclusao")
            {
                if (visaoCarteira.Exclusao && Carteiras.Exists(x => x == carteira))
                    Carteiras.Remove(carteira);
                irParaPaginaInicial();
            }
            else if (e.PropertyName == "Salvamento")
            {
                if (!Carteiras.Exists(x => x == carteira))
                    Carteiras.Add(carteira);
                OnPropertyChanged("Carteiras");
            }
        }

        private void irParaPaginaInicial()
        {
            Pagina.Content = new CarteiraInicio();
        }
    }
}
