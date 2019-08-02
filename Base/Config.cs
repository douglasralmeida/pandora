using Execucao;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace Base
{
    public class Config
    {
        private string _usuarioNome;

        private readonly Dictionary<string, Variavel> _entradas;

        private string _dirtrabalho;

        /* Local onde serão gravadas os arquivos de saída */
        public string DirTrabalho
        {
            get => _dirtrabalho;

            set
            {
                if (value != _dirtrabalho)
                    _dirtrabalho = value;
            }
        }

        /* Tempo em ms de intervalo entre operações */
        public int Intervalo { get; set; }

        public string UsuarioNome => _usuarioNome;

        public Config()
        {
            _dirtrabalho = Environment.GetEnvironmentVariable("USERPROFILE") + "\\Desktop\\Pandora\\";
            Intervalo = 600;
            _usuarioNome = "Usuário sem nome";
        }
    }
}
