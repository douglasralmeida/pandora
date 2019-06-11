using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace Dialogo
{
    /// <summary>
    /// Lógica interna para Senha.xaml
    /// </summary>
    public partial class SenhaDialogo : Window
    {
        public delegate bool ChecarSenhaProc(byte[] hash);

        public ChecarSenhaProc checarSenha;

        public SenhaDialogo()
        {
            InitializeComponent();
        }

        private void BtoOk_Click(object sender, RoutedEventArgs e)
        {
            byte[] hash;

            if (EditSenha.Password.Length > 0)
            {
                hash = gerarHash();
                DialogResult = checarSenha(hash);
            }
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private byte[] gerarHash()
        {
            SHA512 sha = new SHA512Managed();
            return sha.ComputeHash(Encoding.ASCII.GetBytes(EditSenha.Password));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EditSenha.Focus();
        }
    }
}
