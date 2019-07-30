using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace Base
{
    public class Objeto : INotifyPropertyChanged, IEquatable<Objeto>
    {
        protected string _nome;

        protected string nomeElementoXml;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Nome
        {
            get => _nome;

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

        public bool Equals(Objeto outro)
        {
            return null != outro && Nome.ToUpper() == outro.Nome.ToUpper();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Objeto);
        }

        public virtual XElement gerarXml()
        {
            XElement objeto;

            objeto = new XElement(nomeElementoXml);

            return objeto;
        }

        public override int GetHashCode()
        {
            return Nome.ToUpper().GetHashCode();
        }

        public virtual string[] obterEntradas()
        {
            return null;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}