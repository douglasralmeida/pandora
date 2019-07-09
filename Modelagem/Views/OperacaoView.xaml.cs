using Base;
using Execucao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public Modulo ModuloUtilizado { get; set; }

        public string ComandoSelecionado { get; set; }

        public int Id { get; }

        public string Parametros { get; set; }

        private Operacao _operacao;

        public OperacaoView(Operacao operacao)
        {
            InitializeComponent();
            Id = operacao.Id;
            ComandoSelecionado = operacao.Nome;
            Parametros = operacao.Parametros;
            _operacao = operacao;
            DataContext = this;
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtoOk_Click(object sender, RoutedEventArgs e)
        {
            int l;
            string param;

            if (comboComando.SelectedIndex >= 0)
            {
                _operacao.Nome = ComandoSelecionado;
                param = Parametros.Trim();
                l = param.Length - 1;
                if (param[0] != '"' || param[l] != '"')
                    _operacao.Parametros = _operacao.Nome + " \"" + Parametros + "\"";
                else
                    _operacao.Parametros = _operacao.Nome + " " + Parametros;
                DialogResult = true;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            comboComando.Focus();
        }
    }
}
