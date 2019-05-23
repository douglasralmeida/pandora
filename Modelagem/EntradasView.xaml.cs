using System.Windows;

namespace Modelagem
{
    /// <summary>
    /// Lógica interna para EntradasView.xaml
    /// </summary>
    public partial class EntradasView : Window
    {
        public string Entradas
        {
            get; set;
        }

        public string Dir
        {
            get; set;
        }

        public EntradasView(string entradas, string dir)
        {
            InitializeComponent();
            Entradas = entradas;
            Dir = dir;
            editEntradas.Text = Entradas;
            editDir.Text = Dir;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtoOk_Click(object sender, RoutedEventArgs e)
        {
            Entradas = editEntradas.Text;
            Dir = editDir.Text;

            DialogResult = true;
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
