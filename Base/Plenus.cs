using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;

namespace Base
{
    class Plenus
    {
        private string _argumentosIniciais;

        private string _localExecutavel;

        private const string arqc = "comandos.txt";
        Conversor conversor;
        private IntPtr handlePai = IntPtr.Zero;
        private IntPtr handleTerminal = IntPtr.Zero;
        private string telaObtida;
        private List<Atividade> atividades;

        public Plenus()
        {
            conversor = new Conversor();
            conversor.carregarFiltros();
            atividades = new List<Atividade>();
            carregarComandos();

            _argumentosIniciais = "cv3.plc";
            _localExecutavel = "C:\\Wplenus\\Plenus.exe";
        }

        public void abrirPrograma()
        {
            Process.Start(_localExecutavel, _argumentosIniciais);
            Thread.Sleep(3000);
        }

        private void carregarComandos()
        {
            //Atividade atividade;
            String[] comandos = File.ReadAllLines(arqc);
            String[] param;

            foreach (String comando in comandos)
            {
                param = comando.Split('|');
                //atividade = new Atividade(param[0]);
                //atividade.setDescricao(param[1]);
                //atividade.setComando(param[2]);
                //atividades.Add(atividade);
            }
        }

        internal void executarAtividade(Atividade atividade)
        {
            //comando.enviar(atividade.getComandoProcessado());
        }

        public void inserirTexto(string texto)
        {
            Clipboard.SetText(texto);
            Clipboard.Flush();
            menuSimular(2, 2); //clicar em Editar->Copiar
        }

        public bool encontrarJanela()
        {
            handlePai = WinAPI.encontrarJanela(IntPtr.Zero, "OWL_Window");
            if (handlePai == IntPtr.Zero)
                return false;
            handleTerminal = WinAPI.encontrarJanela(handlePai, null);
            if (handleTerminal == IntPtr.Zero)
                return false;
            handleTerminal = WinAPI.encontrarJanela(handleTerminal, null);
            if (handleTerminal == IntPtr.Zero)
                return false;
            handleTerminal = WinAPI.encontrarJanela(handleTerminal, null);
            if (handleTerminal != IntPtr.Zero)
            {
                //comando = new Comando(handlePai);
            }

            return handleTerminal != IntPtr.Zero;
        }

        public void logar(string[] credencial)
        {
            foreach (string s in credencial)
            {
                //comando.enviarTexto(s);
            }
        }

        private void menuSimular(int submenu, int itemmenu)
        {
            Menu.clicar(handlePai, submenu, itemmenu);
        }

        public List<Atividade> obterAtividades()
        {
            return this.atividades;
        }
        public void obterTextoTela()
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            String texto;

            menuSimular(2, 3); //clicar em Editar->Sel. Tudo
            menuSimular(2, 1); //clicar em Editar->Copiar
            System.Threading.Thread.Sleep(1000); //dormir 1 segundo
            texto = Clipboard.GetData("Text").ToString();
            telaObtida = ascii.GetString((new UnicodeEncoding()).GetBytes(texto));
        }

        public string textoObtido()
        {
            return processarTexto(telaObtida);
        }

        private string processarTexto(string texto)
        {
            return conversor.processarPlenus(texto);
        }
        public void enviarTexto(string texto)
        {
            //comando.enviarTexto(texto);
        }
    }
}