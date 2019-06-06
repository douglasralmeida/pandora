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
        public String Nome { get; private set; }

        public String Responsavel { get; set; }

        public byte[] Hash { get; private set; }

        public ObservableCollection<Entrada> Lista { get; }

        public bool Nova { get; set; }
        public bool Exclusao { get; internal set; }

        public Carteira()
        {
            Lista = new ObservableCollection<Entrada>();
            Nome = "Nova carteira";
            Responsavel = "";
            Nova = true;
        }

        public bool Carregar(byte[] hash)
        {
            return false;
        }

        public void Salvar(string value)
        {

        }

        public void setHash()
        {

        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
