using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;

namespace Modelagem
{
    public class EntradaArgumento
    {
        public string Nome { get; set; }

        public string Valor { get; set; }

        public EntradaArgumento()
        {
            Nome = "";
            Valor = "";
        }
    }

    public class EntradaItem
    {
        public ObservableCollection<EntradaArgumento> Lista { get; set; }

        public EntradaItem()
        {
            Lista = new ObservableCollection<EntradaArgumento>();
        }
    }

    public class EntradaItems
    {
        public ObservableCollection<EntradaItem> Lista { get; set; }

        public string[] Cabecalho { get; }

        public EntradaItems(string[] cabecalho)
        {
            Cabecalho = cabecalho;
            Lista = new ObservableCollection<EntradaItem>();            
        }
    }

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

        public DataTable Dados;

        public EntradasView(string[] cabecalho, string[,] dados)
        {
            int j;
            int numlinhas = dados.GetLength(0);
            InitializeComponent();

            Dados = new DataTable("entradas");
            foreach (string s in cabecalho)
                Dados.Columns.Add(s);
            for (int i = 0; i < numlinhas; i++)
            {
                DataRow linha = Dados.NewRow();
                j = 0;
                foreach(DataColumn coluna in Dados.Columns)
                {
                    linha[coluna] = dados[i, j];
                    j++;
                }
                Dados.Rows.Add(linha);
            }
            DataContext = Dados;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtoOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void exibirEntradasNecessarias()
        {
            StringBuilder sb = new StringBuilder();

            //editEntradas.Clear();
            foreach(string s in EntradasNecessarias)
            {
                sb.Append(s);
                sb.Append('=');
                sb.Append('\n');
            }
            //editEntradas.Text = sb.ToString();
        }
    }
}
