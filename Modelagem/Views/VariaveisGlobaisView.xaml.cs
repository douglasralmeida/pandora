using Execucao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Modelagem.Views
{
    public class VarItem
    {
        public string Desc { get; private set; }

        public int Id { get; set; }

        public string Nome { get; private set; }

        public string Valor { get; set; }

        public VarItem(string nome, string desc, string val)
        {
            Nome = nome;
            Desc = desc;
            Valor = val;
        }
    }

    /// <summary>
    /// Lógica interna para VariaveisGlobaisView.xaml
    /// </summary>
    public partial class VariaveisGlobaisView : Window
    {
        public List<VarItem> ItensVariaveis { get; }

        public VariaveisGlobaisView()
        {
            InitializeComponent();
            ItensVariaveis = new List<VarItem>();
            DataContext = this;
        }

        public void abrirVar()
        {
            VarItem vi;
            int i = 0;
            string valor;
            VarGlobais varGlobais;

            KeyValuePair<string, ConstanteInfo>[] ctes = BibliotecaPadrao.Biblioteca.obterCtesChaves();
            varGlobais = (Application.Current as App).VarGlobais;
            ItensVariaveis.Clear();
            foreach (KeyValuePair<string, ConstanteInfo> c in ctes)
            {
                if (c.Value.individual)
                    continue;
                valor = varGlobais.obterVariavel(c.Key);
                vi = new VarItem(c.Key, c.Value.descricao, valor);
                vi.Id = i;
                ItensVariaveis.Add(vi);
                i++;
            }
        }

        private void BtoOk_Click(object sender, RoutedEventArgs e)
        {
            salvarVar();
            DialogResult = true;
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void salvarVar()
        {
            VarGlobais varGlobais = (Application.Current as App).VarGlobais;
            foreach (VarItem item in ItensVariaveis)
                varGlobais.alterarVariavel(item.Nome, item.Valor);
            varGlobais.Salvar();
        }
    }
}
