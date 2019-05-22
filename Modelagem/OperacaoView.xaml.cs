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

namespace Modelagem
{
    /// <summary>
    /// Lógica interna para OperacaoView.xaml
    /// </summary>
    public partial class OperacaoView : Window
    {
        private readonly Base.Operacao _operacao;
        private readonly Base.Operacao _copia;

        public OperacaoView(Base.Operacao operacao)
        {
            InitializeComponent();
            _operacao = operacao;
            _copia = new Base.Operacao(operacao.Id, operacao.Comando, operacao.Parametros);
            //_copia.colarDe(_operacao);
            DataContext = _operacao;
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            _operacao.colarDe(_copia);
            this.DialogResult = false;
        }

        private void BtoOk_Click(object sender, RoutedEventArgs e)
        {
            if (_operacao.Comando.Length > 0)
                this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            editComando.Focus();
            editComando.SelectAll();
        }

    }
}
