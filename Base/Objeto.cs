using System.Xml.Linq;

namespace Base
{
    class Objeto
    {
        protected string nomeElementoXml;

        public Objeto()
        {
            nomeElementoXml = "";
        }

        public virtual bool analisarXml(XElement xml)
        {
            return true;
        }
        public virtual XElement gerarXml()
        {
            //XAttribute[] vazio = { };
            //List<XAttribute> builder = new List<XAttribute>();
            //builder.Add(new XAttribute("nomeatributo", valor);
            //return builder.ToArray();

            return null;
        }
    }
}