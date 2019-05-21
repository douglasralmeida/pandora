using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Base
{
    public class Operacao : Objeto
    {
        private const string TAREFA_INVALIDA = "O pacote informado possui dados de operações inválidos.";

        private int _id;

        private string _comando;

        private string _parametros;

        public Operacao(int id, string comando, string parametros)
        {
            _id = id;
            _comando = comando;
            _parametros = parametros;
            
        }

        public Operacao(XElement xml, int i)
        {
            _id = i;
            analisarXml(xml);
        }

        public string Comando
        {
            get
            {
                return _comando;
            }

            set
            {
                if (_comando != value)
                {
                    _comando = value;
                    OnPropertyChanged("Comando");
                }
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (_id != value)
                    _id = value;
            }
        }

        public string Parametros
        {
            get
            {
                return _parametros;
            }

            set
            {
                if (_parametros != value)
                {
                    _parametros = value;
                    OnPropertyChanged("Parametros");
                }
            }
        }

        protected override void analisarXml(XElement xml)
        {
            char[] separadores = { ' ' };
            string[] comandos;
            string[] elementosnecessarios = { "comando" };

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, TAREFA_INVALIDA);
            comandos = xml.Element("comando").Value.Split(separadores, 2);
            if (comandos.Count() > 0)
                _comando = comandos[0];
            if (comandos.Count() > 1)
                _parametros = comandos[1];
        }

        public override void colarDe(Objeto origem)
        {
            Operacao objeto;

            objeto = (Operacao)origem;
            _id = objeto.Id;
            Comando = objeto.Comando;
            Parametros = objeto.Parametros;
        }

        public override string ToString()
        {
            return _comando + " " + _parametros;
        }
    }
}