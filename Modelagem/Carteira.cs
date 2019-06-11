using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Modelagem.Views
{
    public class Carteira
    {
        private string responsavel;

        public String Nome { get; private set; }

        public String Responsavel
        {
            get => responsavel;

            private set
            {
                responsavel = value;
                Nome = "Carteira de " + value;
            }
        }

        public Dictionary<string, byte[]> Dados { get; }

        public bool Nova { get; set; }

        public bool Exclusao { get; internal set; }
        
        public Carteira()
        {
            string[] ctes;

            Dados = new Dictionary<string, byte[]>();
            responsavel = "";
            Nome = "Nova carteira";
            Nova = true;

            ctes = BibliotecaPadrao.Biblioteca.obterCtesString();
            foreach (string s in ctes)
            {
                Dados.Add(s, new byte[1]);
            }
        }

        public void alterarItem(string nome, byte[] hash, string valor)
        {
            byte[] dado = Encoding.ASCII.GetBytes(valor);
            Dados[nome] = ProtectedData.Protect(dado, hash, DataProtectionScope.CurrentUser);
        }

        public bool Carregar(byte[] hash)
        {
            Nova = false;
            return true;
        }

        public string obterItem(string nome, byte[] hash)
        {
            byte[] valor;

            if (Nova)
                return "";
            else
            {
                try
                {
                    valor = ProtectedData.Unprotect(Dados[nome], hash, DataProtectionScope.CurrentUser);
                    return new string(Encoding.ASCII.GetChars(valor));
                }
                catch
                {
                    return "";
                }
                
            }
        }

        public void Salvar(string novoresponsavel)
        {
            Nova = false;
            Responsavel = novoresponsavel;
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
