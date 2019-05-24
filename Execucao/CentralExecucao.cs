using Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public delegate void ObjetoCarregarAntesEventHandler(object sender);

    //
    // Resumo:
    //     Representa o método que manipulará o evento Execucao.CentralExecucao.ObjetoCarregarDepois,
    //     gerado após uma tarefa ser carregada para o fluxo de execução.
    //
    // Parâmetros:
    //   sender:
    //     A fonte do evento.
    public delegate void ObjetoCarregarDepoisEventHandler(object sender);

    public class CentralExecucao
    {
        private List<Modulo> _biblioteca;

        private Fluxo _fluxo;

        private Dictionary<string, Variavel> _entradas;

        public event ObjetoCarregarAntesEventHandler ObjetoCarregarAntes;

        public event ObjetoCarregarDepoisEventHandler ObjetoCarregarDepois;

        public Dictionary<string, Variavel> Entradas
        {
            get
            {
                return _entradas;
            }
            set
            {
                foreach (KeyValuePair<String, Variavel> e in value)
                {
                    _entradas.Add(e.Key, e.Value);
                }
            }
        }

        public CentralExecucao(List<Modulo> biblioteca)
        {
            _fluxo = new Fluxo(1);
            _biblioteca = biblioteca;
            _entradas = new Dictionary<string, Variavel>();
        }

        public void carregar(Objeto objeto)
        {
            Processo processo;

            OnObjetoCarregarAntes();
            if (objeto is Tarefa)
            {
                processo = new Processo("Processo Genérico");
                processo.Atividades.Add(objeto);
            }
            else
            {
                processo = (Processo)objeto;
            }
            incluirNoFluxo(processo);

            OnObjetoCarregarDepois();
        }

        private List<Comando> comandosDeTarefa(Tarefa tarefa)
        {
            int i;
            string[] parametros;
            StringBuilder builder = new StringBuilder();
            Comando comando;
            List<Comando> comandos = new List<Comando>();
            Funcao funcao;

            foreach (Operacao op in tarefa.Operacoes)
            {
                tarefa.Modulo.Funcoes.TryGetValue(op.Comando, out funcao);
                comando = new Comando(op.Comando, funcao);
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
                    i++;
                }
                comandos.Add(comando);
            }

            return comandos;
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
                        _fluxo.Instrucoes.Add(c);
                }
            }
        }

        protected void OnObjetoCarregarAntes()
        {
            ObjetoCarregarAntes?.Invoke(this);
        }

        protected void OnObjetoCarregarDepois()
        {
            ObjetoCarregarDepois?.Invoke(this);
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
                _entradas.TryGetValue(m.Groups[1].ToString(), out v);
                builder.Replace(s, v.Valor);
            }            
        }

        public void processar()
        {
            
        }
    }
}
