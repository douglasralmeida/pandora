﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;

namespace Modelagem
{
    public static class StringHelper
    {
        public static IEnumerable<string> SplitCSV(this string input)
        {
            Regex csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)", RegexOptions.Compiled);
            foreach (Match match in csvSplit.Matches(input))
            {
                yield return match.Value.TrimStart(',');
            }
        }
    }

    /// <summary>
    /// Lógica interna para EntradasView.xaml
    /// </summary>
    public partial class EntradasView : Window
    {
        private const string DADOS_MODIFICADOS = "Os dados da tabela foram modificados. Você deseja salvar as alterações?";

        private const string DADOS_SEMNOME = "sem título";

        private const string ERRO_CSVINVALIDO = "Ocorreu um erro ao abrir o arquivo informado. Ele pode estar num formato inválido.";

        private const string ERRO_CSVVAZIO = "Ocorreu um erro ao abrir o arquivo informado. O arquivo está vazio.";

        private const string FILTRO_CSV = "Tabela CSV|*.csv|Demais arquivos|*.*";

        private DataTable dados;

        private bool foiSalvo = false;

        private string[] EntradasNecessarias
        {
            get; set;
        }

        public string Dir
        {
            get; set;
        }

        public bool Modificado { get; set; }

        public string NomeArquivo { get; set; }

        public DataTable Dados
        {
            get => dados;

            set
            {
                if (dados != value)
                {
                    dados = value;
                    DataContext = dados;
                }
            }
        }

        public EntradasView(string[] cabecalho, string[,] dados)
        {
            int j;
            int numlinhas = dados.GetLength(0);
            InitializeComponent();

            Dados = new DataTable("entradas");
            foreach (string s in cabecalho)
                Dados.Columns.Add(s);
            for (int i = 0; i < numlinhas; i++)
            {
                DataRow linha = Dados.NewRow();
                j = 0;
                foreach(DataColumn coluna in Dados.Columns)
                {
                    linha[coluna] = dados[i, j];
                    j++;
                }
                Dados.Rows.Add(linha);
            }
            Modificado = false;
            NomeArquivo = DADOS_SEMNOME;
            Dados.RowChanged += Dados_RowChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtoOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtoCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Dados_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            Modificado = true;
        }

        private void exibirEntradasNecessarias()
        {
            StringBuilder sb = new StringBuilder();

            //editEntradas.Clear();
            foreach(string s in EntradasNecessarias)
            {
                sb.Append(s);
                sb.Append('=');
                sb.Append('\n');
            }
            //editEntradas.Text = sb.ToString();
        }

        private void BtoImportar_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialogoAbrir = new OpenFileDialog();

            if (podeLimpar(sender, e))
            {    
                dialogoAbrir.Filter = FILTRO_CSV;
                if (dialogoAbrir.ShowDialog() == true)
                    abrirDados(dialogoAbrir.FileName);
            }
        }

        private void BtoNovo_Click(object sender, RoutedEventArgs e)
        {
            if (podeLimpar(sender, e))
            {
                novosDados();
            }
        }

        private void BtoSalvar_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialogoSalvar = new SaveFileDialog();

            if (NomeArquivo == DADOS_SEMNOME)
            {
                dialogoSalvar.FileName = NomeArquivo;
                dialogoSalvar.Filter = FILTRO_CSV;
                if (dialogoSalvar.ShowDialog() == true)
                {
                    if (salvarDados(dialogoSalvar.FileName))
                    {
                        NomeArquivo = dialogoSalvar.FileName;
                    }
                }
            }
            else
                salvarDados(NomeArquivo);
        }
        
        private bool podeLimpar(object sender, RoutedEventArgs e)
        {
            if (Modificado)
            {
                if (CaixaDialogo.PerguntaSimples(DADOS_MODIFICADOS))
                {
                    if (NomeArquivo == DADOS_SEMNOME)
                    {
                        foiSalvo = false;
                        BtoSalvar_Click(sender, e);
                        if (!foiSalvo)
                            return false;
                    }
                    else
                        if (!salvarDados(NomeArquivo))
                            return false;
                }
            }

            return true;
        }

        private void novosDados()
        {
            NomeArquivo = DADOS_SEMNOME;
            Modificado = false;
            Dados.Clear();
        }

        private bool abrirDados(string arquivo)
        {
            bool extensaocsv;
            int i, j;
            int numlinhas;
            string[] linhas;
            DataTable csvArquivo = new DataTable("entradas");

            //Carrega o arquivo para a memória
            try
            {
                linhas = File.ReadAllLines(arquivo);
            }
            catch (Exception e)
            {
                CaixaDialogo.ErroSimples(e.Message);
                return false;
            }

            //Checa se possui extensão CSV
            extensaocsv = (Path.GetExtension(arquivo) == ".csv");

            //Se o arquivo estiver vazio, não carrega
            numlinhas = linhas.Length;
            if (numlinhas == 0)
            {
                CaixaDialogo.ErroSimples(ERRO_CSVVAZIO);
                return false;
            }

            //Se for arquivo CSV carrega os nomes das colunas
            if (extensaocsv)
            {
                i = 1;
                foreach (string s in linhas[0].SplitCSV())
                    csvArquivo.Columns.Add(s.Replace(@"""", ""));
            }
            else
            {
                i = 0;
                foreach (DataColumn col in Dados.Columns)
                    csvArquivo.Columns.Add(col.Caption);
            }

            //Carrega o conteúdo do arquivo
            for (; i < numlinhas; i++)
            {
                DataRow linha = csvArquivo.NewRow();
                if (extensaocsv)
                {
                    j = 0;
                    foreach (string s in linhas[i].SplitCSV())
                    {
                        if (j >= csvArquivo.Columns.Count)
                        {
                            CaixaDialogo.ErroSimples(ERRO_CSVINVALIDO);
                            return false;
                        }
                        DataColumn coluna = csvArquivo.Columns[j];
                        linha[coluna] = s.Replace(@"""", "");
                        j++;
                    }
                }
                else
                {
                     DataColumn coluna = csvArquivo.Columns[0];
                     linha[coluna] = linhas[i];
                }
                csvArquivo.Rows.Add(linha);
            }

            //Tudo certo
            Dados = csvArquivo;
            foiSalvo = false;
            Modificado = false;
            NomeArquivo = arquivo;

            return true;
        }

        private bool salvarDados(string arquivo)
        {
            StringBuilder sb = new StringBuilder();
            List<string> linhas = new List<string>();

            foreach(DataColumn coluna in Dados.Columns)
            {
                sb.Append('"');
                sb.Append(coluna.Caption);
                sb.Append('"');
                sb.Append(',');
            }
            sb.Remove(sb.Length - 1, 1);
            linhas.Add(sb.ToString());

            foreach (DataRow linha in Dados.Rows)
            {
                sb.Clear();
                foreach (DataColumn coluna in Dados.Columns)
                {
                    sb.Append('"');
                    sb.Append(linha[coluna].ToString());
                    sb.Append('"');
                    sb.Append(',');
                }
                sb.Remove(sb.Length - 1, 1);
                linhas.Add(sb.ToString());
            }
            try
            {
                File.WriteAllLines(arquivo, linhas.ToArray());
            }
            catch
            {
                return false;
            }
            foiSalvo = true;
            Modificado = false;

            return true;
        }

        private void BtoColar_Click(object sender, RoutedEventArgs e)
        {
            string[] celula;
            string clipboardados = Clipboard.GetData(DataFormats.Text).ToString();
            string[] linhas = clipboardados.Split('\n');
            int linhainicial, colunainicial;

            linhainicial = 0;
            colunainicial = GradeDados.SelectedCells[0].Column.DisplayIndex;

            for (int i = 0; i < linhas.Length; i++)
            {
                celula = linhas[i].Split('\t');
                for (int j = 0; i < celula.Length; j++)
                {
                    Dados.Rows[i].SetField(j, celula[j]);
                }
            }
                
//                GradeDados[GradeDados.SelectedIndex, i] = celulas[i];
        }
    }
}
