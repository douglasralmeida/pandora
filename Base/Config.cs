using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    class Config
    {
        private string nomeUsuario;

        public Config()
        {
            nomeUsuario = "Usuário sem nome";
        }

        public string getNomeUsuario()
        {
            return this.nomeUsuario;
        }
    }
}
