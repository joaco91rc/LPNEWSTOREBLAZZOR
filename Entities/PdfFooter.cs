using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Entities
{
    public class PdfFooter : PdfPageEventHelper
    {
        public string Telefono { get; set; }
        public string Instagram { get; set; }
        public string Email { get; set; }

        public override void OnEndPage(PdfWriter writer, iTextSharp.text.Document document)
        {
            PdfPTable footerTbl = new PdfPTable(1);
            footerTbl.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;

            PdfPCell cell = new PdfPCell();
            cell.Border = iTextSharp.text.Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            BaseColor forestGreenCustom = new BaseColor(34, 139, 34);
            // Font styles
            iTextSharp.text.Font verde = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, forestGreenCustom);
            iTextSharp.text.Font negro = FontFactory.GetFont(FontFactory.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            iTextSharp.text.Font negroBold = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 8, BaseColor.BLACK);

            Phrase phrase = new Phrase();

            // Línea 1: "Gracias por su confianza" en verde
            phrase.Add(new Chunk("Gracias por su confianza\n", verde));
            phrase.Add(new Chunk("\n"));

            // Línea 2: Detalles con etiquetas en negrita y valores normales
            phrase.Add(new Chunk("Teléfono: ", negroBold));
            phrase.Add(new Chunk(Telefono + " - ", negro));

            phrase.Add(new Chunk("Instagram: ", negroBold));
            phrase.Add(new Chunk(Instagram + " - ", negro));

            phrase.Add(new Chunk("Email: ", negroBold));
            phrase.Add(new Chunk(Email, negro));

            cell.Phrase = phrase;
            footerTbl.AddCell(cell);

            footerTbl.WriteSelectedRows(
                                         0, -1,
                                         document.LeftMargin,
                                         45f, // Sube el footer un poco
                                         writer.DirectContent
                                     );
        }

    }
}
