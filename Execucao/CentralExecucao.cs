using Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Execucao
{
    public class ExecucaoOpcoes
    {
        public int Atraso { get; set; }

        public ExecucaoOpcoes()
        {

        }
    }

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
        ///   Fluxos que contem listas de tarefas a serem executadas
        /// </summary>
        public List<Fluxo> execucao;
        /* 1. cada condicional gera dois fluxos na lista */

        /// <summary>
        /// Resumo:
        ///   Fluxo contem a lista de tarefas a serem executadas na fase preliminar
        /// </summary>
        public Fluxo preexecucao;

        /// <summary>
        /// Resumo:
        ///   Fluxo contem a lista de tarefas a serem executadas na fase final
        /// </summary>
        public Fluxo posexecucao;

        /// <summary>
        /// Resumo:
        ///   Variáveis são determinadas em tempo de execução, na carteira e na lista de variáveis globais.
        /// </summary>
        public Variaveis variaveis;

        /// Resumo:
        ///   Dados contém as entradas da execução.
        /// </summary>
        public Entradas entradas;
    };

    public class CentralExecucao
    {
        private Instancia _instancia;

        public event EventHandler CompilacaoAntes;

        public event EventHandler CompilacaoDepois;

        public Erros Erros { get; set; }

        public int IntervaloExecucao
        {
            get => Opcoes.Atraso;

            set => Opcoes.Atraso = value;
        }

        public List<Modulo> ModulosUtilizados { get; private set; }

        public Objeto ObjetoCarregado { get; private set; }

        public event ObjetoCarregarAntesEventHandler ObjetoCarregarAntes;

        public event ObjetoCarregarDepoisEventHandler ObjetoCarregarDepois;

        public ExecucaoOpcoes Opcoes { get; private set; }

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
            Opcoes = new ExecucaoOpcoes();
            TotalExitos = 0;
            TotalFalhas = 0;
        }

        public void adicionarVariaveis(string nome, Variavel var)
        {
            _instancia.variaveis.adicionar(nome, var);
        }

        public bool carregar(Objeto objeto)
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

            //verifica a existencia de referências circulares (ciclos)
            if (checarCiclos(processo))
            {
                Erros.Adicionar("SX0001", new string[0]);
                return false;
            }

            // verifica quais módulos são utilizados pelas tarefas e subprocessos
            checarModulos(processo);

            // verifica quais variáveis globais são necessárias para cada módulo utilizado
            // e marca aquelas que não estão disponíveis
            checarCtesNecessarias();

            // se houve erros na checagem, cancela o carregamento
            if (Erros.Quantidade > 0)
                return false;

            // gera os fluxos de execução de acordo com a quantidade de entradas
            prepararFluxos();

            // inclui as atividades do processo no fluxo de execução
            incluirProcesso(processo);

            ObjetoCarregado = objeto;
            OnObjetoCarregarDepois(objeto);

            return true;
        }

        private bool checarCiclos(Processo processo)
        {
            return processo.possuiCiclo();
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

        private List<Comando> comandosDeTarefa(Tarefa tarefa, int entrada)
        {
            int i;
            StringBuilder builder = new StringBuilder();
            Comando comando;
            List<Comando> comandos = new List<Comando>();
            FuncaoInfo funcaoinfo;
            Funcao funcao;
            string parametroProcessado;

            Debug.WriteLine("Preparar Tarefa: " + tarefa);

            foreach (Operacao op in tarefa.Operacoes)
            {
                tarefa.Modulo.Funcoes.TryGetValue(op.Nome, out funcaoinfo);
                funcao = funcaoinfo.funcao;
                comando = new Comando(op.Nome, funcao, Opcoes);
                comando.Dados = _instancia.variaveis;

                Debug.WriteLine(" Parametros antes: " + op.Parametros);

                // listaParametros[0] é o nome do comando
                for (i = 1; i < op.ListaParametros.Length; i++)
                {
                    //substitui o parâmetro por uma variável ou um valor da entrada
                    parametroProcessado = parseParametro(op.ListaParametros[i], entrada);
                    comando.AddParametro(new Variavel(parametroProcessado));
                }

                Debug.Write(" Parametros depois: " + comando.ListaParametros + "\n");
                comandos.Add(comando);
            }

            return comandos;
        }

        public void definirEntradas(Entradas entradas)
        {
            _instancia.entradas = entradas;
        }

        public void gerarInstancia()
        {
            _instancia.variaveis = new Variaveis();
            _instancia.preexecucao = new Fluxo(0);
            _instancia.preexecucao.VariaveisFluxo = _instancia.variaveis;
            _instancia.execucao = new List<Fluxo>();
            _instancia.posexecucao = new Fluxo(0);
            _instancia.posexecucao.VariaveisFluxo = _instancia.variaveis;
        }

        private void incluirTarefa(Tarefa tarefa, AtividadeFase fase)
        {
            int i = 0;

            switch (fase)
            {
                case AtividadeFase.FasePre:
                    foreach (Comando c in comandosDeTarefa(tarefa, i))
                        _instancia.preexecucao.adicionarComando(c);
                    break;

                case AtividadeFase.FasePos:
                    foreach (Comando c in comandosDeTarefa(tarefa, i))
                        _instancia.posexecucao.adicionarComando(c);
                    break;

                default:
                    foreach (Fluxo f in _instancia.execucao)
                    {
                        foreach (Comando c in comandosDeTarefa(tarefa, i))
                            f.adicionarComando(c);
                        i++;
                    }
                    break;
            }
        }

        private void incluirProcessoRecursivo(Processo processo, AtividadeFase fase)
        {
            foreach (Atividade a in processo.Atividades)
            {
                if (a.ObjetoRelacionado is Processo)
                    incluirProcessoRecursivo((Processo)a.ObjetoRelacionado, fase);
                else
                    incluirTarefa((Tarefa)a.ObjetoRelacionado, fase);
            }
        }

        private void incluirProcesso(Processo processo)
        {
            foreach (Atividade a in processo.Atividades)
            {
                if (a.ObjetoRelacionado is Processo)
                    incluirProcessoRecursivo((Processo)a.ObjetoRelacionado, a.Fase);
                else
                    incluirTarefa((Tarefa)a.ObjetoRelacionado, a.Fase);
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
        
        private string parseParametro(string param, int entrada)
        {
            string[] cabecalhoEntradas;
            string[] dadosEntradas;
            bool possuientradas;
            Tuple<string, string, string>[] tuplas;
            StringBuilder builder = new StringBuilder();
            dynamic valor;
            int i;

            cabecalhoEntradas = _instancia.entradas.ObterCabecalhos();
            if (_instancia.entradas.Quantidade() > 0)
            {
                possuientradas = true;
                dadosEntradas = _instancia.entradas.ObterDados(cabecalhoEntradas)[entrada];
            }
            else
            {
                possuientradas = false;
                dadosEntradas = null;
            }
            tuplas = Parser.analisar(param, true);
            if (tuplas.Length == 0)
            {
                tuplas = new Tuple<string, string, string>[1];
                if (param[0] == '"' && param[param.Length - 1] == '"')
                    tuplas[0] = new Tuple<string, string, string>(param, "", param.Remove(param.Length - 1).Remove(0));
                else
                    tuplas[0] = new Tuple<string, string, string>(param, "", param);
            }
            foreach (Tuple<string, string, string> t in tuplas)
            {
                valor = param;
                if (t.Item2 == "ENTRADA")
                {
                    i = 0;
                    valor = "";
                    if (!possuientradas)
                    {
                        string[] valores = { t.Item3 };
                        Erros.Adicionar("ET0001", valores);
                    }
                    foreach(string s in cabecalhoEntradas)
                    {
                        if (s == t.Item3)
                        {
                            valor = dadosEntradas[i];
                            break;
                        }
                        i++;
                    }
                }
                else if (t.Item2 == "VAR")
                {
                    valor = _instancia.variaveis.obterVar(t.Item3);
                    if (valor == null)
                        valor = "";
                }
                builder.Append(param.Replace(t.Item1, valor));
                builder.Append(' ');
            }
            // remove o espaço extra
            if (builder.Length > 0)
                builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }

        public void preparar()
        {
            //Desabilita o CAPSLOCK parar prevenir erros de digitação
            Teclado.desabilitarCapsLock();
        }

        public void prepararFluxos()
        {
            int quantidadeentradas;

            quantidadeentradas = _instancia.entradas.Quantidade();

            //cria um fluxo de execução para cada entrada de dados
            for (int i = 0; i < quantidadeentradas; i++)
                _instancia.execucao.Add(new Fluxo(i + 1));

            // se não tem entradas, cria pelo menos um fluxo mesmo assim
            if (_instancia.execucao.Count == 0)
                _instancia.execucao.Add(new Fluxo(1));
        }

        public void processar()
        {
            // primeiro, executa os comandos da fase pré-execução
            Debug.WriteLine("Fase Pré-execução");
            Debug.WriteLine("=================");
            _instancia.preexecucao.processar();
            TotalExitos++;

            // segundo, executa os comandos da fase principal
            // um processamento para cada entrada disponível
            Debug.WriteLine("Fase Execução");
            Debug.WriteLine("=================");
            foreach (Fluxo f in _instancia.execucao)
            {
                f.VariaveisFluxo = _instancia.preexecucao.VariaveisFluxo.gerarCopia();
                f.processar();
                TotalExitos++;
            }

            // terceiro, executa os comandos da fase pos-execução
            Debug.WriteLine("Fase Pós-execução");
            Debug.WriteLine("=================");
            _instancia.posexecucao.processar();
            TotalExitos++;
        }
    }
}
