using System.ComponentModel;
using System.Xml.Linq;

namespace Base
{
    public class Objeto : INotifyPropertyChanged
    {
        protected string _nome;

        protected string nomeElementoXml;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Nome
        {
            get
            {
                return _nome;
            }
            set
            {
                if (_nome != value)
                {
                    _nome = value;
                    OnPropertyChanged("Nome");
                }
            }
        }

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

        public virtual void colarDe(Objeto objeto)
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

        protected void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}