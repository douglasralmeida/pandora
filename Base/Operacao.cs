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

        private string _parametros;

        public Operacao(int id, string comando, string parametros)
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
                _nome = comandos[0];
            if (comandos.Count() > 1)
                _parametros = comandos[1];
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
            comando.Value = _nome + " " + _parametros;

            operacao.Add(comando);

            return operacao;
        }

        public override string[] obterEntradas()
        {
            List<string> lista;
            MatchCollection combinacoes;
            string padrao = "{ENTRADA (.*?)}";

            combinacoes = Regex.Matches(_parametros, padrao);
            lista = new List<string>();
            foreach (Match m in combinacoes)
            {
                lista.Add(m.Groups[1].ToString());
            }
            return lista.ToArray();
        }

        public override string ToString()
        {
            return _nome + " " + _parametros;
        }
    }
}