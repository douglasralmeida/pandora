using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base
{
    class Operacao
    {
        private int id;

        private string comando;

        public Operacao(int id, string comando)
        {
            this.id = id;
            this.comando = comando;
        }

        public string getComando()
        {
            return this.comando;
        }

        public int getId()
        {
            return this.id;
        }
    }
}
