using Base;
using Modelagem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Modelagem
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Carteiras _carteiras;

        private Config _config;

        private const string NOMEAPLICACAO = "Modelagem de Processos do Pandora";

        public string NomeAplicacao
        {
            get => NOMEAPLICACAO;
        }

        public Carteiras Carteiras => _carteiras;

        public Config Configuracoes
        {
            get => _config;
        }

        public App()
        {
            _carteiras = new Carteiras();
            _carteiras.Carregar();
            _config = new Config();
        }
    }
}
