using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Execucao
{
    /* CONSTANTEINFO
     * descricao  = Descrição da constante
     * oculta     = Trata a constante como senha
     * individual = Constante dever ser salva numa carteira
     */
    public struct ConstanteInfo
    {
        public string descricao;
        public bool individual;
        public bool obrigatoria;
        public bool oculta;

        public ConstanteInfo(string desc, bool indiv, bool oc, bool ob)
        {
            descricao = desc;
            individual = indiv;
            oculta = oc;
            obrigatoria = ob;
        }
    }


    public struct FuncaoInfo {
        public Funcao funcao;
        public int numArgumentos;

        public FuncaoInfo(Funcao _func, int numArg)
        {
            funcao = _func;
            numArgumentos = numArg;
        }
    }

    public class Modulo
    {
        [DllImport("user32.dll")]
        protected static extern bool SetForegroundWindow(IntPtr hWnd);

        public string Nome
        {
            get; set;
        }

        public Dictionary<string, FuncaoInfo> Funcoes { get; private set; }

        public Dictionary<string, ConstanteInfo> ConstantesNecessarias { get; private set; }

        public Modulo(string nome)
        {
            ConstantesNecessarias = new Dictionary<string, ConstanteInfo>();
            Funcoes = new Dictionary<string, FuncaoInfo>();
            adicionarComandos();
            adicionarConstNecessarias();
            Nome = nome;
        }

        public virtual void adicionarComandos()
        {
            
        }

        public virtual void adicionarConstNecessarias()
        {

        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
