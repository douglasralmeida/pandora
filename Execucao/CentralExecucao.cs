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
        public Fluxo fluxo;
        public Dictionary<string, Variavel> variaveis;
        public Dictionary<string, dynamic> dados;
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

        public Dictionary<string, Variavel> Variaveis
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
            _instancia.variaveis.Add(nome, var);
        }

        public void carregar(Objeto objeto)
        {
            Processo processo;

            OnObjetoCarregarAntes(objeto);
            if (objeto is Tarefa)
            {
                //Tarefa tarefa = (Tarefa)objeto;
                processo = new Processo("Processo Genérico", null, null);
                processo.Atividades.Add(objeto);
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

        private void checarModulos(Processo processo)
        {
            ModulosUtilizados.Clear();
            foreach (Objeto o in processo.Atividades)
            {
                if (o is Tarefa)
                {
                    Tarefa t = (Tarefa)o;
                    if (!ModulosUtilizados.Contains(t.Modulo))
                        ModulosUtilizados.Add(t.Modulo);
                } else
                {
                    Processo p = (Processo)o;
                    checarModulos(p);
                }
            }
        }

        private List<Comando> comandosDeTarefa(Tarefa tarefa)
        {
            int i;
            string[] parametros;
            StringBuilder builder = new StringBuilder();
            Comando comando;
            List<Comando> comandos = new List<Comando>();
            FuncaoInfo funcaoinfo;
            Funcao funcao;

            foreach (Operacao op in tarefa.Operacoes)
            {
                tarefa.Modulo.Funcoes.TryGetValue(op.Nome, out funcaoinfo);
                funcao = funcaoinfo.funcao;
                comando = new Comando(op.Nome, funcao);

                Debug.Write("Parametros antes: " + op.Parametros);

                parametros = op.Parametros.Split(' ');
                i = 0;
                foreach (string param in parametros)
                {
                    //remove as aspas
                    builder.Clear();
                    builder.Append(param);
                    builder.Remove(0, 1);
                    builder.Remove(parametros.Length - 1, 1);

                    //substitui pelas variáveis de entrada
                    parseParametros(builder);
                    comando.Parametros.Add(new Variavel(builder.ToString()));

                    Debug.Write("Parametros depois: " + comando.Parametros);

                    i++;
                }
                comandos.Add(comando);
            }

            return comandos;
        }

        public bool compilar()
        {
            OnCompilacaoAntes(new EventArgs());

            foreach (Modulo m in ModulosUtilizados)
            {
                foreach (KeyValuePair<string, ConstanteInfo> c in m.ConstantesNecessarias)
                {
                    if (c.Value.obrigatoria)
                    {
                        if (!Variaveis.ContainsKey(c.Key))
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
            if (Erros.Quantidade > 0)
                return false;

            OnCompilacaoDepois(new EventArgs());
            return true;
        }

        public void gerarInstancia()
        {
            _instancia.dados = new Dictionary<string, dynamic>();
            _instancia.variaveis = new Dictionary<string, Variavel>();
            _instancia.fluxo = new Fluxo(1);
            _instancia.fluxo.Dados = _instancia.dados;
            _instancia.fluxo.Variaveis = _instancia.variaveis;
        }

        private void incluirNoFluxo(Processo processo)
        {
            Tarefa tarefa;

            foreach (Objeto o in processo.Atividades)
            {
                // Quando for implementado o suporte a desvios condicionais,
                // deverá ser adicionado um novo fluxo dentro do fluxo atual
                // e as operações pertencentes ao desvio deverão ser adicionados
                // dentro do novo fluxo adicionado.
                // Também será adicionado um novo comando IrParaFluxo(numero);
                if (o is Processo)
                    incluirNoFluxo((Processo)o);
                else
                {
                    tarefa = (Tarefa)o;
                    foreach (Comando c in comandosDeTarefa(tarefa))
                        _instancia.fluxo.Instrucoes.Add(c);
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

        private void parseParametros(StringBuilder builder)
        {
            MatchCollection combinacoes;
            string padrao = "{ENTRADA (.*?)}";
            string s;
            Variavel v;

            combinacoes = Regex.Matches(builder.ToString(), padrao);
            foreach (Match m in combinacoes)
            {
                s = string.Format("{{ENTRADA {0}}}", m.Groups[1]);
                _instancia.variaveis.TryGetValue(m.Groups[1].ToString(), out v);
                builder.Replace(s, v.Valor);
            }            
        }

        public void processar()
        {
            _instancia.fluxo.processar();
            TotalExitos++;
        }
    }
}
