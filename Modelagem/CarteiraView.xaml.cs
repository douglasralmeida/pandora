using Execucao;
using Modelagem.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Modelagem
{
    /// <summary>
    /// Interação lógica para CarteiraView.xam
    /// </summary>
    /// 

    public class CarteiraItem
    {
        public string Desc { get; private set; }

        public int Id { get; set; }

        public string Nome { get; private set; }

        public bool Oculto { get; private set; }

        public string Valor { get; set; }

        public CarteiraItem(string nome, string desc, string val, bool ocul)
        {
            Nome = nome;
            Desc = desc;
            Valor = val;
            Oculto = ocul;
        }
    }

    public partial class CarteiraView : UserControl
    {
        const string EXCLUIR_CATEIRA = "Você tem certeza que deseja excluir esta carteira?";
        const string ERRO_NOMERESPONSAVEL_VAZIO = "O nome do responsável da carteira atual não foi informado.";
        const string ERRO_SENHAS_DIFERENTES = "As senhas informadas estão diferentes.";

        public bool Exclusao { get; private set; }

        Carteira carteira;

        List<CarteiraItem> itens;

        public event PropertyChangedEventHandler PropertyChanged;

        public string NomeCarteira
        {
            get => carteira.ToString();
        }

        public List<CarteiraItem> ItensCarteira
        {
            get => itens;
        }

        public bool PodeExcluir
        {
            get => !carteira.Nova;
        }

        public string Responsavel
        {
            get => carteira.Responsavel;

            set => carteira.Responsavel = value;
        }

        public CarteiraView(Carteira carteira)
        {
            int i = 0;
            Modulo[] modulos;

            InitializeComponent();
            Exclusao = false;
            this.carteira = carteira;
            modulos = BibliotecaPadrao.Biblioteca.ObterTudo;
            itens = new List<CarteiraItem>();
            foreach (Modulo mod in modulos)
            {
                foreach (KeyValuePair<string, ConstanteInfo> par in mod.ConstantesNecessarias)
                {
                    CarteiraItem ci = new CarteiraItem(mod.Nome + "." + par.Key, par.Value.descricao, "", par.Value.oculta);
                    ci.Id = i;
                    itens.Add(ci);
                    i++;
                }
            }
            //ItensCarteira.Keys
            //ItensCarteira.Values
            DataContext = this;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            int i;

            if (this.DataContext != null)
            {
                i = (int)((PasswordBox)sender).Tag;
                ((dynamic)this.DataContext).ItensCarteira[i].Valor = ((PasswordBox)sender).Password;
            }
        }

        private void BtoExcluirCarteira_Click(object sender, RoutedEventArgs e)
        {
            if (CaixaDialogo.PerguntaSimples(EXCLUIR_CATEIRA))
            {
                Exclusao = true;
                OnPropertyChanged("Exclusao");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtoSalvarCarteira_Click(object sender, RoutedEventArgs e)
        {
            if (carteira.Responsavel.Trim().Length == 0)
            {
                CaixaDialogo.ErroSimples(ERRO_NOMERESPONSAVEL_VAZIO);
                return;
            }

            if (CaixaSenha1.SecurePassword != CaixaSenha2.SecurePassword)
            {
                CaixaDialogo.ErroSimples(ERRO_SENHAS_DIFERENTES);
                return;
            }
            salvarCarteira();
        }

        private void salvarCarteira()
        {
            int i;
            byte[] hash, dados, dadosseguros;

            dados = Encoding.ASCII.GetBytes(CaixaSenha1.Password);
            SHA512 sha = new SHA512Managed();
            hash = sha.ComputeHash(dados);

            foreach (CarteiraItem item in itens)
            {
                dadosseguros = ProtectedData.Protect(info, hash, DataProtectionScope.CurrentUser);
            }
        }
    }
}
