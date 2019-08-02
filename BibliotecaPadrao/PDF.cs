using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BibliotecaPadrao
{
    public static class PDF
    {
        public static void gerarDeTexto(string texto, string nomearquivo)
        {
            Stream arquivo = new FileStream(nomearquivo, FileMode.Create);
            PdfWriter writer = new PdfWriter(arquivo, new WriterProperties().SetPdfVersion(PdfVersion.PDF_2_0));
            PdfDocument pdfDocument = new PdfDocument(writer);

            pdfDocument.SetTagged();
            Document document = new Document(pdfDocument);
            document.Add(new Paragraph(texto));
            document.Close();
        }
    }
}
