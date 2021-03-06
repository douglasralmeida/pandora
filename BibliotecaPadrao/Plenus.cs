﻿using Base;
using Conversores;
using Execucao;
using Modelagem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;

namespace BibliotecaPadrao
{
    public class Plenus : Modulo
    {
        private Funcao _funcaoAbrirPrograma = (vars, args, opcoes) =>
        {
            IntPtr handle;
            int pid;
            dynamic saida;
            string localarquivos;
            string nomeexe;
            string nomeconfig;
            string arquivoplenus = "pandoracv3.plc";

            // Obter as variáveis globais
            saida = vars.obterVar("PLENUS_EXE", false);
            if (saida != null)
                nomeexe = saida;
            else
                return (false, "Uma variável global era esperada, mas não foi encontrada.");
            saida = vars.obterVar("PLENUS_CONFIG", false);
            if (saida != null)
                nomeconfig = saida;
            else
                return (false, "Uma variável global era esperada, mas não foi encontrada.");
            saida = vars.obterVar("PLENUS_LOCAL", false);
            if (saida != null)
                localarquivos = saida;
            else
                return (false, "Uma variável global era esperada, mas não foi encontrada.");

            //Gera os arquivos de configuração do Plenus
            try
            {
                string configdir = Directory.GetCurrentDirectory() + "\\config\\";
                File.Copy(configdir + "cv3-par.config", localarquivos + arquivoplenus);
            }
            catch (IOException e)
            {
                return (false, "Ocorreu um erro ao copiar o arquivo de configuração do Plenus");
            }

            //Armazena o handle da janela e o pid para manipulações futuras
            (handle, pid) = Auxiliar.executarPrograma(nomeexe, arquivoplenus, localarquivos, 3000);
            if (handle != IntPtr.Zero)
            {
                vars.adicionar("plenus.handle", false, new Variavel(handle));
                vars.adicionar("plenus.pid", false, new Variavel(pid));
                return (true, null);
            }
            else
            {
                return (false, "O Plenus não foi aberto corretamente.");
            }
        };

        // sem argumentos
        // usa variáveis: PLENUS_USUARIO, PLENUS_MAT, PLENUS_SENHA, plenus.handle
        private Funcao _funcaoAutenticar = (vars, args, opcoes) =>
        {
            StringBuilder builder = new StringBuilder();
            dynamic saida;
            string usuario;
            string matricula = "";
            string senha = "";
            dynamic handle;

            saida = vars.obterVar("PLENUS_USUARIO", false);
            if (saida != null)
                usuario = saida;
            else
                return (false, "A variável global 'PLENUS_USUARIO' era esperada, mas não foi encontrada.");
            saida = vars.obterVar("PLENUS_MAT", false);
            if (saida != null)
                matricula = saida;
            else
                return (false, "A variável global 'PLENUS_MAT' era esperada, mas não foi encontrada.");
            saida = vars.obterVar("PLENUS_SENHA", false);
            if (saida != null)
                senha = saida;
            else
                return (false, "A variável global 'PLENUS_SENHA' era esperada, mas não foi encontrada.");
            handle = vars.obterVar("plenus.handle", false);
            if (handle != null)
            {
                IntPtr p = handle;
                SetForegroundWindow(p);

                builder.Append(usuario);
                builder.Append(Teclado.TEC_ENTER);
                System.Windows.Forms.SendKeys.SendWait(builder.ToString());
                Thread.Sleep(1000);
                builder.Clear();

                builder.Append(matricula);
                builder.Append(Teclado.TEC_TAB);    
                builder.Append(senha);
                builder.Append(Teclado.TEC_ENTER);
                System.Windows.Forms.SendKeys.SendWait(builder.ToString());

                return (true, null);
            }

            return (false, "Uma janela do Plenus era esperada, mas não foi encontrada.");
        };

        // sem argumentos
        // não usa variáveis
        private Funcao _funcaoCopiarTela = (vars, args, opcoes) =>
        {
            IConversor conversor;
            int resultado;
            dynamic handle;
            string dados;

            try
            {
                conversor = new PlenusConversor();
            }
            catch
            {
                return (false, "Não foi possível carregar os filtros de conversão do Plenus.");
            }
            handle = vars.obterVar("plenus.handle", false);
            if (handle != null)
            {
                //clicar em Editar->Sel. Tudo
                resultado = Base.Menu.clicar(handle, 2, 3);
                if (resultado != 0)
                    return (false, "Erro.");

                //clicar em Editar->Copiar
                resultado = Base.Menu.clicar(handle, 2, 1);
                if (resultado != 0)
                    return (false, "Erro.");

                //dorme 1 seg aguardando processamento
                Thread.Sleep(1000);

                //Cola em uma string para limpeza de lixo da tela Plenus
                dados = Clipboard.GetText();
                dados = conversor.processar(dados);
                Clipboard.SetText(dados);

                return (true, null);
            }

            return (false, "Uma janela do Plenus era esperada, mas não foi encontrada.");
        };

        // arg1 = texto a digitar na tela
        // usa a variável plenus.handle
        private Funcao _funcaoDigitar = (vars, args, opcoes) =>
        {
            IEnumerator<string> lista;
            string texto;
            dynamic handle;

            if (args.Cont < 1)
                return (false, "A operação Digitar esperava 1 argumento, mas ele não foi encontrado.");
            lista = args.GetEnumerator();
            lista.MoveNext();
            texto = lista.Current;
            handle = vars.obterVar("plenus.handle", false);
            if (handle != null)
            {
                IntPtr p = handle;
                SetForegroundWindow(p);
                System.Windows.Forms.SendKeys.SendWait(texto);
                return (true, null);
            }

            return (false, "Uma janela do Plenus era esperada, mas não foi encontrada.");
        };

        // sem argumentos
        // usa a constante plenus.pid
        private Funcao _funcaoEncerrarPrograma = (vars, args, opcoes) =>
        {
            dynamic pid;

            pid = vars.obterVar("plenus.pid", false);
            if (pid != null)
            {
                int id = pid;
                if (id == 0)
                    return (false, "Era esperado um PID válido do Plenus para fechar.");
                Auxiliar.encerrarPrograma(id);
                return (true, null);
            }

            return (false, "Era esperado o programa Plenus para fechar.");
        };

        // dois argumentos: tipoextensao, nomearquivo
        // usa variáveis: global.dirtrabalho
        private Funcao _funcaoSalvarTela = (vars, args, opcoes) =>
        {
            IConversor conversor;
            IEnumerator<string> lista;
            bool extok = false;
            string[] extaceitaveis = { "pdf", "txt" };
            int resultado;
            dynamic handle;
            string dirtrabalho;
            string nomearquivo;
            string ext;
            string texto;

            if (args.Cont < 2)
                return (false, "A operação SalvarTela esperava 2 argumentos, mas eles não foram encontrados.");
            try
            {
                conversor = new PlenusConversor();
            }
            catch
            {
                return (false, "Não foi possível carregar os filtros de conversão do Plenus.");
            }
            lista = args.GetEnumerator();
            lista.MoveNext();
            ext = lista.Current;
            foreach(string s in extaceitaveis)
                extok |= (s == ext);
            if (!extok)
                return (false, "O primeiro parâmetro da operação SalvarTela está incorreto. Era esperado: pdf ou txt.");
            lista.MoveNext();
            nomearquivo = lista.Current;
            dirtrabalho = vars.obterVar("global.dirtrabalho", false);
            if (dirtrabalho == null)
                return (false, "O diretório de trabalho do Pandora não foi configurado.");
            handle = vars.obterVar("plenus.handle", false);
            if (handle != null)
            {
                //clicar em Editar->Sel. Tudo
                resultado = Base.Menu.clicar(handle, 2, 3);
                if (resultado != 0)
                    return (false, "Erro.");

                //clicar em Editar->Copiar
                resultado = Base.Menu.clicar(handle, 2, 1);
                if (resultado != 0)
                    return (false, "Erro.");

                //dorme 1 seg aguardando processamento
                System.Threading.Thread.Sleep(1000);

                //obtém o texto da área de transferência
                texto = Clipboard.GetText();

                //Limpeza de lixo da tela Plenus
                texto = conversor.processar(texto);

                //salva em um arquivo PDF ou TXT
                try
                {
                    if (ext == "pdf")
                        PDF.gerarDeTexto(texto, dirtrabalho + nomearquivo);
                    else
                        TXT.gerarDeTexto(texto, dirtrabalho + nomearquivo);
                }
                catch (DirectoryNotFoundException)
                {
                    return (false, "O diretório de trabalho especificado está inacessível ou não existe.");
                }

                return (true, null);
            }

            return (false, "Uma janela do Plenus era esperada, mas não foi encontrada.");
        };

        public Plenus() : base("Plenus")
        {
            
        }

        public override void adicionarComandos()
        {
            base.adicionarComandos();
            Funcoes.Add("AbrirPrograma", new FuncaoInfo(_funcaoAbrirPrograma, 0));
            Funcoes.Add("Autenticar", new FuncaoInfo(_funcaoAutenticar, 0));
            Funcoes.Add("CopiarTela", new FuncaoInfo(_funcaoCopiarTela, 0));
            Funcoes.Add("Digitar", new FuncaoInfo(_funcaoDigitar, 1));
            Funcoes.Add("FecharPrograma", new FuncaoInfo(_funcaoEncerrarPrograma, 0));
            Funcoes.Add("SalvarTela", new FuncaoInfo(_funcaoSalvarTela, 2));
        }

        public override void adicionarConstNecessarias()
        {
            base.adicionarConstNecessarias();

            ConstantesNecessarias.Add("PLENUS_USUARIO", new ConstanteInfo("Usuário de acesso ao Plenus", true, false, true));
            ConstantesNecessarias.Add("PLENUS_SENHA", new ConstanteInfo("Senha de acesso ao Plenus", true, true, true));

            ConstantesNecessarias.Add("PLENUS_EXE", new ConstanteInfo("Nome do arquivo executável do Plenus", false, true, true));
            ConstantesNecessarias.Add("PLENUS_IP", new ConstanteInfo("IP do servidor CV3 para acesso via Plenus", false, true, true));
            ConstantesNecessarias.Add("PLENUS_LOCAL", new ConstanteInfo("Diretório dos arquivos de configuração do Plenus", false, true, true));
        }
    }
}