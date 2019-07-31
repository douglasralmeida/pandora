using System;
using System.ComponentModel;
using System.Xml.Linq;

namespace Base
{
    public struct ObjetoBackup
    {
        internal string nome;
    };

    public class Objeto : INotifyPropertyChanged, IEquatable<Objeto>, IEditableObject
    {
        protected string _nome;

        protected ObjetoBackup copiaDados;

        protected bool erroExiste = false;

        protected string erroDescricao = "";

        protected bool modoTransacao = false;

        protected string nomeElementoXml;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Nome
        {
            get => _nome;

            set
            {
                if (_nome != value)
                {
                    try
                    {
                        BeginEdit();
                        _nome = value;
                        OnPropertyChanged("Nome");
                        EndEdit();
                        erroExiste = false;
                        OnPropertyChanged("ErroExiste");
                    }
                    catch (Exception e)
                    {
                        CancelEdit();
                        erroDescricao = e.Message;
                        erroExiste = true;
                        OnPropertyChanged("ErroExiste");
                    }
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

        public void BeginEdit()
        {
            if (!modoTransacao)
            {
                copiaDados.nome = _nome;
                modoTransacao = true;
            }
        }

        public void CancelEdit()
        {
            if (modoTransacao)
            {
                _nome = this.copiaDados.nome;
                modoTransacao = false;
            }
        }

        public virtual void colarDe(Objeto objeto)
        {
            
        }

        public void EndEdit()
        {
            if (modoTransacao)
            {
                this.copiaDados.nome = "";
                modoTransacao = false;
            }
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