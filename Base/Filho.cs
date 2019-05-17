using System.Collections.Generic;

namespace Base
{
    class Filho : Objeto
    {
        protected List<Filho> filhos;

        public Filho()
        {
            filhos = new List<Filho>();
        }
    }
}
