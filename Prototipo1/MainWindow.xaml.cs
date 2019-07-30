using Base;
using BibliotecaPadrao;
using System;
using System.IO;
using System.Text;
using System.Windows;

namespace Prototipo1
{

    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string texto;

            texto = Clipboard.GetText();
            File.WriteAllText("saida.txt", texto, new ASCIIEncoding());
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            String texto;

            texto = File.ReadAllText("saida.txt", new ASCIIEncoding());
            textoExibir.Text = texto;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            String texto;

            texto = processar(textoExibir.Text);
            textoExibir.Text = texto;
        }

        private string processar(String texto)
        {
            Conversor conv;

            conv = new Conversor();
            conv.carregarFiltros();

            return conv.processarPlenus(texto);
        }
    }
}
