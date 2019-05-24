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

        private readonly ObservableCollection<Variavel> _entradas;

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

        public ObservableCollection<Variavel> Entradas
        {
            get
            {
                return _entradas;
            }
        }

        public Config()
        {
            _dirtrabalho = "%USERPROFILE%\\Desktop\\Pandora";
            _entradas = new ObservableCollection<Variavel>();
            _usuarioNome = "Usuário sem nome";
        }

        public void setEntradasFromString(string entradas)
        {
            _entradas.Clear();
            using (StringReader sr = new StringReader(entradas))
            {
                string linha;
                while ((linha = sr.ReadLine()) != null)
                    _entradas.Add(new Variavel(linha));
            }
        }
    }
}
