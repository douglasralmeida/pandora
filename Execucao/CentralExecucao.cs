﻿using Base;
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

        public event ObjetoCarregarAntesEventHandler ObjetoCarregarAntes;

        public event ObjetoCarregarDepoisEventHandler ObjetoCarregarDepois;

        public Dictionary<string, Variavel> Variaveis
        {
            get
            {
                return _instancia.variaveis;
            }
            set
            {
                foreach (KeyValuePair<String, Variavel> k in value)
                {
                    _instancia.variaveis.Add(k.Key, k.Value);
                }
            }
        }

        public CentralExecucao()
        {
            
        }

        public void carregar(Objeto objeto)
        {
            Processo processo;

            OnObjetoCarregarAntes(objeto);
            if (objeto is Tarefa)
            {
                processo = new Processo("Processo Genérico", null, null);
                processo.Atividades.Add(objeto);
            }
            else
            {
                processo = (Processo)objeto;
            }
            incluirNoFluxo(processo);
            OnObjetoCarregarDepois(objeto);
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
                tarefa.Modulo.Funcoes.TryGetValue(op.Nome, out funcao);
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
        }
    }
}
