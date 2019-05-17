using Base;
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
            nomeArquivo = "sem nome.pandorapac";
            modificado = true;
        }

        public bool abrir(string nomeArquivo)
        {
            XElement xml;

            xml = XElement.Load(nomeArquivo);
            if (processarXML(xml))
            {
                modificado = false;
                this.nomeArquivo = nomeArquivo;
                return true;
            }

            return false;
        }

        private XElement gerarXML()
        {
            XElement xml;

            xml = pacote.dataToXml();

            return xml;
        }

        public bool getModificado()
        {
            return modificado;
        }

        public string getNomeArquivo()
        {
            return nomeArquivo;
        }

        private void novo(string usuarioGerador)
        {
            pacote = new Pacote(usuarioGerador);
        }

        private bool processarXML(XElement xml)
        {
            pacote = new Pacote("sem nome");

            return pacote.xmlToData(xml);
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
