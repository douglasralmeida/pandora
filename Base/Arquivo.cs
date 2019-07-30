using System;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Web.Script.Serialization;

namespace Base
{
    public class Arquivo
    {
        private IsolatedStorageFile storage;

        public Arquivo()
        {
            storage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
        }

        public ObservableCollection<T> processarLista<T>(string nomeArquivo)
        {
            Type t = new ObservableCollection<T>().GetType();
            if (storage.FileExists(nomeArquivo))
            {
                using (IsolatedStorageFileStream arquivoJson = new IsolatedStorageFileStream(nomeArquivo, FileMode.Open, storage))
                {
                    using (StreamReader leitor = new StreamReader(arquivoJson))
                    {
                        string json = leitor.ReadToEnd();
                        return (ObservableCollection<T>)(new JavaScriptSerializer().Deserialize(json, t));
                    }
                }
            }
            else
            {
                return new ObservableCollection<T>();
            }
        }

        public void salvarLista<T>(ObservableCollection<T> lista, string nomeArquivo)
        {
            using (IsolatedStorageFileStream arquivoJson = new IsolatedStorageFileStream(nomeArquivo, FileMode.Create, storage))
            {
                string json = new JavaScriptSerializer().Serialize(lista);
                using (StreamWriter escritor = new StreamWriter(arquivoJson))
                {
                    escritor.Write(json);
                }
            }
        }
    }
}
