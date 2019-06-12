using Modelagem.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Modelagem
{
    public class Carteiras
    {
        private const string ARQ_CARTEIRA = "carteira.json";

        private IsolatedStorageFile storage;

        public ObservableCollection<Carteira> Lista { get; private set; } = null;

        public Carteiras()
        {
            storage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
        }

        public void AdicionarCarteira(Carteira carteira)
        {
            if (!Lista.Contains(carteira))
                Lista.Add(carteira);
            Salvar();
        }

        public void Carregar()
        {
            if (storage.FileExists(ARQ_CARTEIRA))
            {
                processarArquivoCarteira();
            }
            else
            {
                Lista = new ObservableCollection<Carteira>();
            }
        }

        private void processarArquivoCarteira()
        {
            Type t = (new ObservableCollection<Carteira>()).GetType();
            using (IsolatedStorageFileStream arquivoJson = new IsolatedStorageFileStream(ARQ_CARTEIRA, FileMode.Open, storage))
            {
                using (StreamReader leitor = new StreamReader(arquivoJson))
                {
                    string json = leitor.ReadToEnd();
                    Lista = (ObservableCollection<Carteira>)(new JavaScriptSerializer().Deserialize(json, t));
                }
            }            
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
            Type t = Lista.GetType();
            using (IsolatedStorageFileStream arquivoJson = new IsolatedStorageFileStream(ARQ_CARTEIRA, FileMode.Create, storage))
            {
                string json = new JavaScriptSerializer().Serialize(Lista);
                using (StreamWriter escritor = new StreamWriter(arquivoJson))
                {
                    escritor.Write(json);
                }
            }
        }
    }
}