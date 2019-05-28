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
        public ProcessoView() : base()
        {
            InitializeComponent();
            //_processo = processo;
            
        }

        protected void AtividadeDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Base.Objeto objeto = ((ListViewItem)sender).Content as Base.Objeto;

            if (objeto is Base.Tarefa)
                editarTarefa(objeto);
            else if (objeto is Base.Processo)
                exibirProcesso(objeto);
        }

        private void exibirProcesso(Objeto objeto)
        {
            ObjetoAtivo = objeto;
        }

        private void editarTarefa(Objeto objeto)
        {
            throw new NotImplementedException();
        }
    }
}
