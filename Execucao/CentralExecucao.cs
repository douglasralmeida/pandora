using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Execucao
{
    //
    // Resumo:
    //     Representa o método que manipulará o evento Execucao.CentralExecucao.ObjetoCarregarAntes,
    //     gerado antes de uma tarefa ser carregada para o fluxo de execução.
    //
    // Parâmetros:
    //   sender:
    //     A fonte do evento.
    //   objeto:
    //     O objeto que será carregado
    public delegate void ObjetoCarregarAntesEventHandler(object sender, Objeto objeto);

    //
    // Resumo:
    //     Representa o método que manipulará o evento Execucao.CentralExecucao.ObjetoCarregarDepois,
    //     gerado após uma tarefa ser carregada para o fluxo de execução.
    //
    // Parâmetros:
    //   sender:
    //     A fonte do evento.
    //   objeto:
    //     O objeto que foi carregado
    public delegate void ObjetoCarregarDepoisEventHandler(object sender, Objeto objeto);

    struct Instancia
    {
        /// <summary>
        /// Resumo:
        ///   Fluxo contem a lista de tarefas a serem executadas
        /// </summary>
        public Fluxo fluxo;

        /// <summary>
        /// Resumo:
        ///   Variáveis são determinadas em tempo de execução, na carteira e na lista de variáveis globais.
        /// </summary>
        public Variaveis variaveis;

        /// <summary>
        /// Resumo:
        ///   Dados contém as entradas da execução.
        /// </summary>
        public Dictionary<string, dynamic>[] entradas;
    };

    public class CentralExecucao
    {
        private Instancia _instancia;

        public event EventHandler CompilacaoAntes;

        public event EventHandler CompilacaoDepois;

        public Erros Erros { get; set; }

        public List<Modulo> ModulosUtilizados { get; private set; }

        public Objeto ObjetoCarregado { get; private set; }

        public event ObjetoCarregarAntesEventHandler ObjetoCarregarAntes;

        public event ObjetoCarregarDepoisEventHandler ObjetoCarregarDepois;

        public int TotalExitos { get; private set; }

        public int TotalFalhas { get; private set; }

        public Variaveis ListaVariaveis
        {
            get
            {
                return _instancia.variaveis;
            }
        }

        public CentralExecucao()
        {
            ModulosUtilizados = new List<Modulo>();
            TotalExitos = 0;
            TotalFalhas = 0;
        }

        public void adicionarVariaveis(string nome, Variavel var)
        {
            _instancia.variaveis.adicionar(nome, var);
        }

        public void carregar(Objeto objeto)
        {
            Processo processo;

            OnObjetoCarregarAntes(objeto);
            if (objeto is Tarefa)
            {
                //Tarefa tarefa = (Tarefa)objeto;
                processo = new Processo("Processo Genérico", null, null);
                processo.Atividades.Add(new Atividade(objeto));
            }
            else
            {
                processo = (Processo)objeto;
            }
            incluirNoFluxo(processo);
            checarModulos(processo);
            ObjetoCarregado = objeto;
            OnObjetoCarregarDepois(objeto);
        }

        private void checarCtesNecessarias()
        {
            foreach (Modulo m in ModulosUtilizados)
            {
                foreach (KeyValuePair<string, ConstanteInfo> c in m.ConstantesNecessarias)
                {
                    if (c.Value.obrigatoria)
                    {
                        if (!ListaVariaveis.contemVar(c.Key))
                        {
                            string tipo;
                            string[] nome = new string[1];

                            nome[0] = c.Key;
                            if (c.Value.individual)
                                tipo = "CT0002";
                            else
                                tipo = "VG0001";
                            Erros.Adicionar(tipo, nome);
                        }
                    }
                }
            }
        }

        private void checarEntradas()
        {

        }

        private void checarModulos(Processo processo)
        {
            ModulosUtilizados.Clear();
            foreach (Atividade a in processo.Atividades)
            {
                if (a.ObjetoRelacionado is Tarefa)
                {
                    Tarefa t = (Tarefa)a.ObjetoRelacionado;
                    if (!ModulosUtilizados.Contains(t.Modulo))
                        ModulosUtilizados.Add(t.Modulo);
                } else
                {
                    Processo p = (Processo)a.ObjetoRelacionado;
                    checarModulos(p);
                }
            }
        }

        private List<Comando> comandosDeTarefa(Tarefa tarefa)
        {
            int i;
            StringBuilder builder = new StringBuilder();
            Comando comando;
            List<Comando> comandos = new List<Comando>();
            FuncaoInfo funcaoinfo;
            Funcao funcao;
            string parametroProcessado;

            Debug.WriteLine("Tarefa: " + tarefa);

            foreach (Operacao op in tarefa.Operacoes)
            {
                tarefa.Modulo.Funcoes.TryGetValue(op.Nome, out funcaoinfo);
                funcao = funcaoinfo.funcao;
                comando = new Comando(op.Nome, funcao);
                comando.Dados = _instancia.variaveis;

                Debug.WriteLine("Parametros antes: " + op.Parametros);

                // listaParametros[0] é o nome do comando
                for (i = 1; i < op.ListaParametros.Length; i++)
                {
                    //substitui pelas variáveis de entrada
                    parametroProcessado = parseParametro(op.ListaParametros[i]);
                    comando.Parametros.Add(new Variavel(parametroProcessado));
                }

                Debug.Write("Parametros depois: ");
                foreach (Variavel v in comando.Parametros)
                {
                    Debug.Write(v.ToString());
                    Debug.Write("; ");
                }
                Debug.Write("\n");

                comandos.Add(comando);
            }

            return comandos;
        }

        public bool compilar()
        {
            OnCompilacaoAntes(new EventArgs());

            checarCtesNecessarias();
            checarEntradas();
            if (Erros.Quantidade > 0)
                return false;

            OnCompilacaoDepois(new EventArgs());
            return true;
        }

        public void gerarInstancia(int numEntradas)
        {
            _instancia.entradas = new Dictionary<string, dynamic>[numEntradas];
            _instancia.variaveis = new Variaveis();
            _instancia.fluxo = new Fluxo(1);
            _instancia.fluxo.Entradas = null;
            _instancia.fluxo.VariaveisFluxo = _instancia.variaveis;
        }

        private void incluirNoFluxo(Processo processo)
        {
            Tarefa tarefa;

            foreach (Atividade a in processo.Atividades)
            {
                // Quando for implementado o suporte a desvios condicionais,
                // deverá ser adicionado um novo fluxo dentro do fluxo atual
                // e as operações pertencentes ao desvio deverão ser adicionados
                // dentro do novo fluxo adicionado.
                // Também será adicionado um novo comando IrParaFluxo(numero, posicao);
                if (a.ObjetoRelacionado is Processo)
                    incluirNoFluxo((Processo)a.ObjetoRelacionado);
                else
                {
                    int fase;
                    switch (a.Fase)
                    {
                        case AtividadeFase.FasePre:
                            fase = 0;
                            break;

                        case AtividadeFase.FasePos:
                            fase = 2;
                            break;

                        default:
                            fase = 1;
                            break;
                    }

                    tarefa = (Tarefa)a.ObjetoRelacionado;
                    foreach (Comando c in comandosDeTarefa(tarefa))
                    {
                        _instancia.fluxo.adicionarComando(c, fase);
                    }
                }
            }
        }

        protected virtual void OnCompilacaoAntes(EventArgs e)
        {
            EventHandler handler = CompilacaoAntes;
            CompilacaoAntes?.Invoke(this, e);
        }

        protected virtual void OnCompilacaoDepois(EventArgs e)
        {
            EventHandler handler = CompilacaoDepois;
            CompilacaoDepois?.Invoke(this, e);
        }

        protected void OnObjetoCarregarAntes(Objeto objeto)
        {
            ObjetoCarregarAntes?.Invoke(this, objeto);
        }

        protected void OnObjetoCarregarDepois(Objeto objeto)
        {
            ObjetoCarregarDepois?.Invoke(this, objeto);
        }

        private string parseParametro(string param)
        {
            string[] lista;
            StringBuilder builder = new StringBuilder();
            dynamic valor;


            lista = Parser.analisarVarEntrada(param, true);
            if (lista.Length == 0)
            {
                lista = new string[1];
                if (param[0] == '"' && param[param.Length - 1] == '"')
                {
                    lista[0] = param.Remove(param.Length - 1).Remove(0);
                }
                else
                    lista[0] = param;
            }
            foreach (string s in lista)
            {
                valor = _instancia.variaveis.obterVar(s);
                if (valor == null)
                    builder.Append(s);
                else
                    builder.Replace(s, valor);
                builder.Append(' ');
            }
            // remove o espaço extra
            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        public void processar()
        {
            _instancia.fluxo.processar();
            TotalExitos++;
        }
    }
}
