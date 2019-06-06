using Modelagem.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Modelagem
{
    /// <summary>
    /// Lógica interna para CarteirasView.xaml
    /// </summary>
    public partial class CarteirasView : Window
    {
        CarteiraView visaoCarteira;

        private Carteira carteira;

        private List<Carteira> carteiras;

        public CarteirasView(List<Carteira> carteiras)
        {
            InitializeComponent();
            this.carteiras = carteiras;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            irParaPaginaInicial();
        }

        private void BtoNovaCarteira_Click(object sender, RoutedEventArgs e)
        {
            Carteira carteira = new Carteira();

            visaoCarteira = new CarteiraView(carteira);
            visaoCarteira.PropertyChanged += VisaoCarteira_PropertyChanged;
            Pagina.Content = visaoCarteira;
        }

        private void VisaoCarteira_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Exclusao")
            {
                if (visaoCarteira.Exclusao && carteiras.Exists(x => x == carteira))
                    carteiras.Remove(carteira);
                irParaPaginaInicial();
            }
        }

        private void irParaPaginaInicial()
        {
            Pagina.Content = new CarteiraInicio();
        }
    }
}
