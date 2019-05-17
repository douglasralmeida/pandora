using System.Windows;

namespace Prototipo2
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
            foreach (Atividade atividade in plenus.obterAtividades())
            {
                listaComandos.Items.Add(atividade);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Atividade atividade;

            if (listaComandos.SelectedIndex > -1)
            {
                atividade = listaComandos.SelectedValue as Atividade;
                atividade.adicionarEntrada(editEntrada.Text);
                plenus.executarAtividade(atividade);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            plenus.enviarTexto("INFBEN{ENTER}");
        }
    }
}
