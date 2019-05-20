using System.Linq;
using System.Xml.Linq;

namespace Base
{
    static class XMLAuxiliar
    {
        public static void checarNomeXml(XElement xml, string nome, string msgerro)
        {
            if (xml.Name != nome)
                throw new PandoraException(msgerro);
        }

        public static void checarFilhoXML(XElement xml, string nome, string msgerro)
        {
            if (xml.Elements(nome).Count() == 0)
                throw new PandoraException(msgerro);
        }

        public static void checarFilhosXML(XElement xml, string[] nomes, string msgerro)
        {
            foreach (string nome in nomes)
                checarFilhoXML(xml, nome, msgerro);
        }
    }
}
