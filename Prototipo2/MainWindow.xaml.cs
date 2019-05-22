using Microsoft.Win32;
using System.Windows;
using System.Xml.Linq;

namespace Prototipo2
{

    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        private Base.Plenus _plenus;
        private Base.Pacote _pacote;

        public MainWindow()
        {
            InitializeComponent();
            _plenus = new Base.Plenus();
            DataContext = _pacote;
        }

        public void procurarJanela()
        {
            if (!_plenus.encontrarJanela())
            {
                MessageBox.Show("Janela do Plenus não encontrada.");
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Atividade atividade;

            //if (listaComandos.SelectedIndex > -1)
            //{
            //    atividade = listaComandos.SelectedValue as Atividade;
            //    atividade.adicionarEntrada(editEntrada.Text);
            //    _plenus.executarAtividade(atividade);
            //}
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _plenus.enviarTexto("INFBEN{ENTER}");
        }

        private void BtoAbrirPacote_Click(object sender, RoutedEventArgs e)
        {
            XElement xml;
            OpenFileDialog dialogo = new OpenFileDialog();

            dialogo.Filter = "Pacote do Pandora|*.pandorapac|Demais arquivos|*.*";
            if (dialogo.ShowDialog() == true)
            {
                Title = dialogo.FileName;
                xml = XElement.Load(dialogo.FileName);
                _pacote = new Base.Pacote(xml);
                DataContext = _pacote;
            }
        }

        private void BtoAbrirPlenus_Click(object sender, RoutedEventArgs e)
        {
            _plenus.abrirPrograma();
            _plenus.encontrarJanela();


        }
    }
}
