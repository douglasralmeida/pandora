using Base;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Modelagem
{
    class Editor : INotifyPropertyChanged
    {
        private string nomeArquivo;

        private bool _modificado;

        private Pacote _pacote;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Modificado
        {
            get
            {
                return _modificado;
            }
            set
            {
                if (_modificado != value)
                {
                    _modificado = value;
                    OnPropertyChanged("Modificado");
                }
            }
        }

        public Editor()
        {
            _modificado = false;
        }

        public bool abrir(string nomearquivo)
        {
            XElement xml;

            xml = XElement.Load(nomearquivo);
            if (processarXML(xml))
            {
                nomeArquivo = nomearquivo;
                Modificado = false;
                return true;
            }

            return false;
        }

        private XElement gerarXML()
        {
            XElement xml;

            xml = _pacote.gerarXml();

            return xml;
        }

        public TreeViewItem getArvoreProcessos()
        {
            TreeViewItem item;

            item = new TreeViewItem();
            item.Header = "Todas os processos";

            return item;
        }

        public TreeViewItem getArvoreTarefas()
        {
            TreeViewItem item;

            item = new TreeViewItem();
            item.Header = "Todas as tarefas";
            foreach (Tarefa nome in _pacote.Tarefas)
            {
                item.Items.Add(nome);
            }
            return item;
        }

        public string getNomeArquivo()
        {
            return nomeArquivo;
        }

        public void novo(string usuarioGerador)
        {
            _pacote = new Pacote(usuarioGerador);
            _pacote.PropertyChanged += Pacote_PropertyChanged;
            nomeArquivo = "sem nome.pandorapac";
            Modificado = true;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Pacote_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Modificado = true;
        }

        private bool processarXML(XElement xml)
        {
            _pacote = new Pacote(xml);
            _pacote.PropertyChanged += Pacote_PropertyChanged;

            return true;
        }


        public void salvar(string nomeArquivo)
        {
            XElement xml;

            xml = gerarXML();
            xml.Save(nomeArquivo);
            this.nomeArquivo = nomeArquivo;
            Modificado = false;
        }
    }
}
