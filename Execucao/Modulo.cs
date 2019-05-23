using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    class Modulo
    {
        private ObservableCollection<Comando> _comandos;

        public string Nome
        {
            get; set;
        }

        public ObservableCollection<Comando> Comandos
        {
            get
            {
                return _comandos;
            }
        }

        public Modulo(string nome)
        {
            Nome = nome;
        }
    }
}
