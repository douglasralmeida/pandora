using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Base
{
    class Operacao : Objeto
    {
        private const string TAREFA_INVALIDA = "O pacote informado possui dados de operações inválidos.";

        private int id;

        private string comando;

        public Operacao(int id, string comando)
        {
            this.id = id;
            this.comando = comando;
        }

        public Operacao(XElement xml)
        {

        }

        protected override void analisarXml(XElement xml)
        {
            string[] elementosnecessarios = { "comando" };

            XMLAuxiliar.checarFilhosXML(xml, elementosnecessarios, TAREFA_INVALIDA);
            this.comando = xml.Element("comando").Value;
        }

        public string getComando()
        {
            return this.comando;
        }

        public int getId()
        {
            return this.id;
        }

        public override string ToString()
        {
            return this.comando;
        }
    }
}
