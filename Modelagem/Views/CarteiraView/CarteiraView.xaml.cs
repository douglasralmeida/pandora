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

        public bool MostrarTexto { get => (!Oculto); }

        public bool MostrarOculto { get => (Valor.Length > 0) && Oculto; }

        public bool MostrarVazio { get => (Valor.Length == 0) && Oculto; }

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
        const string ERRO_NOMERESPONSAVEL_VAZIO = "O nome do responsável da carteira atual não foi informado. Informe um nome e tente novamente.";
        const string ERRO_SENHAS_DIFERENTES = "As senhas informadas estão diferentes. Informe a mesma senha em ambos os campos de senha e tente novamente.";
        const string ERRO_RESP_JAEXISTE = "Já existe uma carteira onde seu responsável possui o mesmo nome que o nome informado. Escolha um nome diferente e tente novamente.";

        public bool Exclusao { get; private set; }

        App _app = (Application.Current as App);

        Carteira carteira;

        public event PropertyChangedEventHandler PropertyChanged;

        public string NomeCarteira
        {
            get => carteira.Nome;
        }

        public List<CarteiraItem> ItensCarteira { get; }

        public bool PodeExcluir
        {
            get => !carteira.Nova;
        }

        public string Responsavel { get; set; }

        public CarteiraView(Carteira carteira)
        {
            InitializeComponent();
            Exclusao = false;
            this.carteira = carteira;
            ItensCarteira = new List<CarteiraItem>();
            DataContext = this;
        }

        public void criarCarteira()
        {
            CarteiraItem ci;
            int i = 0;

            KeyValuePair<string, ConstanteInfo>[] ctes = BibliotecaPadrao.Biblioteca.obterCtesChaves();
            Responsavel = "";
            ItensCarteira.Clear();
            foreach (KeyValuePair<string, ConstanteInfo> c in ctes)
            {
                if (!c.Value.individual)
                    continue;
                ci = new CarteiraItem(c.Key, c.Value.descricao, "", c.Value.oculta);
                ci.Id = i;
                ItensCarteira.Add(ci);
                i++;
            }
        }

        public bool abrirCarteira(byte[] hash)
        {
            CarteiraItem ci;
            int i = 0;
            string valor;

            KeyValuePair<string, ConstanteInfo>[] ctes = BibliotecaPadrao.Biblioteca.obterCtesChaves();
            Responsavel = carteira.Responsavel;
            ItensCarteira.Clear();
            valor = carteira.obterItem("PALAVRA_MAGICA", hash);
            if (valor != "!abracadabra1")
                return false;
            foreach (KeyValuePair<string, ConstanteInfo> c in ctes)
            {
                if (!c.Value.individual)
                    continue;

                if (carteira.Dados.ContainsKey(c.Key))
                {
                    valor = carteira.obterItem(c.Key, hash);
                }
                else
                {
                    valor = "";
                }
                ci = new CarteiraItem(c.Key, c.Value.descricao, valor, c.Value.oculta);
                ci.Id = i;
                ItensCarteira.Add(ci);
                i++;
            }

            return true;
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
            if (CaixaDialogo.PerguntaSimples(this, EXCLUIR_CATEIRA))
            {
                Exclusao = true;
                _app.Carteiras.RemoverCarteira(carteira);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void BtoSalvarCarteira_Click(object sender, RoutedEventArgs e)
        {
            if (Responsavel.Trim().Length == 0)
            {
                CaixaDialogo.ErroSimples(this, ERRO_NOMERESPONSAVEL_VAZIO);
                return;
            }
            if (_app.Carteiras.ProcurarPorResponsavel(Responsavel))
            {
                CaixaDialogo.ErroSimples(this, ERRO_RESP_JAEXISTE);
                return;
            }
            if (CaixaSenha1.Password != CaixaSenha2.Password)
            {
                CaixaDialogo.ErroSimples(this, ERRO_SENHAS_DIFERENTES);
                return;
            }
            salvarCarteira();
        }

        private void salvarCarteira()
        {
            byte[] hash;

            SHA512 sha = new SHA512Managed();
            hash  = sha.ComputeHash(Encoding.ASCII.GetBytes(CaixaSenha1.Password));
            foreach (CarteiraItem item in ItensCarteira)
                carteira.alterarItem(item.Nome, hash, item.Valor);
            carteira.alterarItem("PALAVRA_MAGICA", hash, "!abracadabra1");
            carteira.Salvar(Responsavel);
            _app.Carteiras.AdicionarCarteira(carteira);
        }
    }
}
