using Base;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Modelagem
{
    class Editor
    {
        private string nomeArquivo;

        private bool modificado;

        private Pacote pacote;

        public Editor()
        {

        }

        public bool abrir(string nomearquivo)
        {
            XElement xml;

            xml = XElement.Load(nomearquivo);
            if (processarXML(xml))
            {
                modificado = false;
                nomeArquivo = nomearquivo;
                return true;
            }

            return false;
        }

        private XElement gerarXML()
        {
            XElement xml;

            xml = pacote.gerarXml();

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
            List<Tarefa> lista;
            TreeViewItem item;

            lista = pacote.getListaTarefas();
            item = new TreeViewItem();
            item.Header = "Todas as tarefas";
            foreach (Tarefa nome in lista)
            {
                item.Items.Add(nome);
            }
            return item;
        }

        public bool getModificado()
        {
            return modificado;
        }

        public string getNomeArquivo()
        {
            return nomeArquivo;
        }

        public void novo(string usuarioGerador)
        {
            pacote = new Pacote(usuarioGerador);
            nomeArquivo = "sem nome.pandorapac";
            modificado = true;
        }

        private bool processarXML(XElement xml)
        {
            pacote = new Pacote(xml);

            return true;
        }

        public void salvar(string nomeArquivo)
        {
            XElement xml;

            xml = gerarXML();
            xml.Save(nomeArquivo);
            modificado = false;
            this.nomeArquivo = nomeArquivo;
        }
    }
}
