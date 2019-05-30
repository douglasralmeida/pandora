using Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Modelagem
{
    /// <summary>
    /// Interação lógica para ProcessoControl.xam
    /// </summary>
    public partial class ProcessoView : ObjetoView
    {
        private Processo _processoativo => (Processo)_objetoativo;

        public ProcessoView()
        {
            InitializeComponent();
        }

        protected void AtividadeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Objeto objeto = ((ListViewItem)sender).Content as Objeto;

            ObjetoAtivo = objeto;
        }

        private void BtoInserirAtividade_Click(object sender, RoutedEventArgs e)
        {
            Objeto objeto;

            AtividadesView atividadesView = new AtividadesView()
            {
                Owner = Application.Current.MainWindow
            };
            atividadesView.TodasAtividades_Adicionar(TodasTarefas);
            atividadesView.TodasAtividades_Adicionar(TodosProcessos);
            atividadesView.ShowDialog();

            if (atividadesView.DialogResult ?? true)
            {
                objeto = atividadesView.ObjetoSelecionado;
                _processoativo.Atividades.Add(objeto);
            }

        }
    }
}
