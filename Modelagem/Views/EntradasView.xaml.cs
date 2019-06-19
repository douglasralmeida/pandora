using System.Text;
using System.Windows;

namespace Modelagem
{
    /// <summary>
    /// Lógica interna para EntradasView.xaml
    /// </summary>
    public partial class EntradasView : Window
    {
        private string[] EntradasNecessarias
        {
            get; set;
        }

        public string Dir
        {
            get; set;
        }

        public EntradasView(string[] entradasNecessarias, string dir)
        {
            InitializeComponent();
            EntradasNecessarias = entradasNecessarias;
            exibirEntradasNecessarias();

            Dir = dir;
            editDir.Text = Dir;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtoOk_Click(object sender, RoutedEventArgs e)
        {
            Dir = editDir.Text;

            DialogResult = true;
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void exibirEntradasNecessarias()
        {
            StringBuilder sb = new StringBuilder();

            editEntradas.Clear();
            foreach(string s in EntradasNecessarias)
            {
                sb.Append(s);
                sb.Append('=');
                sb.Append('\n');
            }
            editEntradas.Text = sb.ToString();
        }
    }
}
