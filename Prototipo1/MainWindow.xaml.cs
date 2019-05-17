using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace Prototipo1
{

    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private Plenus plenus;

        public MainWindow()
        {
            InitializeComponent();
            plenus = new Plenus();
            if (!plenus.encontrarJanela())
            {
                MessageBox.Show("Janela do Plenus não encontrada.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            plenus.inserirTexto(editClasse.Text);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string texto;

            plenus.obterTextoTela();
            texto = plenus.textoObtido();
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
