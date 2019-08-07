using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BibliotecaPadrao
{
    public class PDF
    {
        public static void mesclar(string[] arquivos, string saida)
        {
            List<PdfDocument> lista = new List<PdfDocument>();
            using (FileStream fluxo = new FileStream(saida, FileMode.Create))
            {
                PdfDocument pdf = new PdfDocument(new PdfWriter(saida));
                PdfMerger mesclador = new PdfMerger(pdf);

                foreach(string arquivo in arquivos)
                {
                    PdfDocument entrada = new PdfDocument(new PdfReader(arquivo));
                    mesclador.Merge(entrada, 1, entrada.GetNumberOfPages());
                    lista.Add(entrada);
                }

                foreach (PdfDocument doc in lista)
                    doc.Close();
                pdf.Close();
            }
        }

        public static void gerarDeTexto(string texto, string nomearquivo)
        {
            Stream arquivo = new FileStream(nomearquivo, FileMode.Create);
            PdfWriter writer = new PdfWriter(arquivo, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfDocument pdfDocument = new PdfDocument(writer);
            Paragraph paragrafo;

            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            paragrafo = new Paragraph(texto);
            paragrafo.SetFontFamily("Consolas");
            paragrafo.SetFontSize(11);
            document.Add(new Paragraph(texto));
            document.Close();
        }
    }
}
