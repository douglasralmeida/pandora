using System.Xml.Linq;

namespace Base
{
    public class Objeto
    {
        protected string nomeElementoXml;

        public Objeto()
        {
            nomeElementoXml = "";
        }

        public Objeto(XElement xml) : base()
        {
            analisarXml(xml);
        }

        protected virtual void analisarXml(XElement xml)
        {
        
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