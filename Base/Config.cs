using Execucao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Base
{
    class Config
    {
        private string _usuarioNome;

        private readonly Dictionary<string, Variavel> _entradas;

        private string _dirtrabalho;

        public string DirTrabalho
        {
            get
            {
                return _dirtrabalho;
            }

            set
            {
                if (value != _dirtrabalho)
                    _dirtrabalho = value;
            }
        }

        public string UsuarioNome
        {
            get
            {
                return _usuarioNome;
            }
        }

        public Dictionary<string, Variavel> Entradas
        {
            get
            {
                return _entradas;
            }
        }

        public Config()
        {
            _dirtrabalho = "%USERPROFILE%\\Desktop\\Pandora";
            _entradas = new Dictionary<string, Variavel>();
            _usuarioNome = "Usuário sem nome";
        }

        public void setEntradasFromString(string entradas)
        {
            string linha;
            string[] par = { };
            
            _entradas.Clear();
            using (StringReader sr = new StringReader(entradas))
            {
                while ((linha = sr.ReadLine()) != null)
                    par = linha.Split('=');
                    _entradas.Add(par[0], new Variavel(par[1]));
            }
        }
    }
}
