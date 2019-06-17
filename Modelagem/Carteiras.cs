using Base;
using Modelagem.Views;
using System.Collections.ObjectModel;
using System.Windows;

namespace Modelagem
{
    public class Carteiras
    {
        private const string ARQ_CARTEIRA = "carteira.json";

        private Arquivo _arquivo;

        public ObservableCollection<Carteira> Lista { get; private set; } = null;

        public Carteiras()
        {
            _arquivo = (Application.Current as App).Arquivo;
        }

        public void AdicionarCarteira(Carteira carteira)
        {
            if (!Lista.Contains(carteira))
                Lista.Add(carteira);
            Salvar();
        }

        public void Carregar()
        {
            Lista = _arquivo.processarLista<Carteira>(ARQ_CARTEIRA);
        }

        public bool ProcurarPorResponsavel(string nome)
        {
            foreach (Carteira c in Lista)
            {
                if (c.Responsavel == nome)
                    return true;
            }

            return false;
        }

        public void RemoverCarteira(Carteira carteira)
        {
            if (Lista.Contains(carteira))
            {
                Lista.Remove(carteira);
                Salvar();
            }
        }

        public void Salvar()
        {
            _arquivo.salvarLista(Lista, ARQ_CARTEIRA);
        }
    }
}