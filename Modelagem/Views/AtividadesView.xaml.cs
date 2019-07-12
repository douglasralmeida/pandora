using Base;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace Modelagem
{
    public class ComparacaoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }
    }

    /// <summary>
    /// Lógica interna para AtividadesView.xaml
    /// </summary>
    public partial class AtividadesView : Window
    {
        public ObservableCollection<Objeto> TodasAtividades { get; private set; }

        public Objeto ObjetoSelecionado
        {
            get;

            private set;
        }
        public AtividadeFase FaseEscolhida { get; set; }

        public AtividadesView()
        {
            InitializeComponent();
            TodasAtividades = new ObservableCollection<Objeto>();
            FaseEscolhida = AtividadeFase.FaseNormal;
            ObjetoSelecionado = null;
            DataContext = this;
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtoOK_Click(object sender, RoutedEventArgs e)
        {
            if (ObjetoSelecionado != null)
                DialogResult = true;
        }

        private void ListaAtividades_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ListaAtividades.SelectedItem != null)
                ObjetoSelecionado = (ListaAtividades.SelectedItem as Objeto);
            else
                ObjetoSelecionado = null;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            editFiltro.Focus();
            editFiltro.SelectAll();
        }

        public void TodasAtividades_Adicionar(ObservableCollection<Processo> lista)
        {
            foreach (Processo p in lista)
            {
                TodasAtividades.Add(p);
            }
        }

        public void TodasAtividades_Adicionar(ObservableCollection<Tarefa> lista)
        {
            foreach (Tarefa t in lista)
            {
                TodasAtividades.Add(t);
            }
        }
    }
}
