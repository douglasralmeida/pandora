using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Base
{
    public class Operacao : Objeto
    {
        private const string TAREFA_INVALIDA = "O pacote informado possui dados de operações inválidos.";

        private int _id;

        private string[] _parametros;

        public Operacao(int id, string comando, string[] parametros)
        {
            nomeElementoXml = "operacao";
            _id = id;
            _nome = comando;
            _parametros = parametros;
        }

        public Operacao(XElement xml, int i)
        {
            nomeElementoXml = "operacao";
            _id = i;
            analisarXml(xml);
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

        public string[] ListaParametros
        {
            get => _parametros;

            set => _parametros = value;
        }

        public string Parametros
        {
            get
            {
                StringBuilder builder = new StringBuilder();

                for (int i = 1; i < _parametros.Length; i++)
                {
                    builder.Append('"');
                    builder.Append(_parametros[i]);
                    builder.Append('"');
                    builder.Append(' ');
                }
                if (builder.Length > 0 && builder[builder.Length-1] == ' ')
                    builder.Remove(builder.Length - 1, 1);

                return builder.ToString();
            }

            set
            {
                char separador = ' ';
                char escape = '"';

                _parametros = Parser.dividirString(value, separador, escape);
                if (_parametros.Count() > 0)
                    _nome = _parametros[0];
                else
                    _nome = "";
                OnPropertyChanged("Parametros");
            }
        }

        protected override void analisarXml(XElement xml)
        {
            string[] elementosnecessarios = { "comando" };

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, TAREFA_INVALIDA);
            Parametros = xml.Element("comando").Value;
        }

        public override void colarDe(Objeto origem)
        {
            Operacao objeto;

            objeto = (Operacao)origem;
            _id = objeto.Id;
            Nome = objeto.Nome;
            Parametros = objeto.Parametros;
        }

        public override XElement gerarXml()
        {
            XElement comando;
            XElement operacao;

            operacao = base.gerarXml();
            comando = new XElement("comando");
            comando.Value = _nome + " " + Parametros;

            operacao.Add(comando);

            return operacao;
        }

        public override string[] obterEntradas()
        {
            Tuple<string, string, string>[] parametros;
            List<string> lista = new List<string>();

            for (int i = 1; i < _parametros.Length; i++)
            {
                parametros = Parser.analisar(_parametros[i], true);
                foreach (Tuple<string, string, string> p in parametros)
                    lista.Add(p.Item3);
            }

            return lista.Distinct().ToArray();
        }

        public override string ToString()
        {
            return _nome + " " + Parametros;
        }
    }
}