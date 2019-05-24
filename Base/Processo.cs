using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using System.Xml.Linq;

namespace Base
{
    public class Processo : Objeto
    {
        private string _descricao;

        private string _nome;

        private ObservableCollection<Objeto> _atividades;

        public ObservableCollection<Objeto> Atividades
        {
            get
            {
                return _atividades;
            }
        }

        public string Descricao
        {
            get
            {
                return _descricao;
            }
            set
            {
                if (_descricao != value)
                {
                    _descricao = value;
                    OnPropertyChanged("Descricao");
                }
            }
        }

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

        public Processo(string nome)
        {
            _nome = nome;
            _atividades = new ObservableCollection<Objeto>();
            _atividades.CollectionChanged += Atividades_CollectionChanged;
        }

        public Processo(XElement xml)
        {

        }

        protected override void analisarXml(XElement xml)
        {

        }

        void Atividades_CollectionChanged(object Sender, NotifyCollectionChangedEventArgs Args)
        {
            OnPropertyChanged("Atividades");
        }
    }
}
