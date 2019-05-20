using System.Collections.Generic;
using System.Xml.Linq;

namespace Base
{
    class Filho : Objeto
    {
        protected List<Filho> filhos;

        public Filho()
        {
            filhos = new List<Filho>();
        }

        public Filho(XElement xml) : base(xml)
        {

        }
    }
}
