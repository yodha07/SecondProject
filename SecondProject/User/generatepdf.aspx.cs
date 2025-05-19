using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.UI;


namespace SecondProject.User
{
    public partial class generatepdf : System.Web.UI.Page
    {
        // Custom class for product
        public class Product
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Example data (replace with actual session values)
            string customerName = "John Doe"; // Session["CustomerName"]
            string customerEmail = Session["Email"].ToString(); // Session["CustomerEmail"]
            string invoiceNumber = GenerateInvoiceNumber().ToString();
            DateTime invoiceDate = DateTime.Now;

            // Read session lists
            List<string> titles = Session["Title"] as List<string>;  // ← Populated in pdfgenerate()
            List<double> prices = Session["Price"] as List<double>;  // ← Populated in pdfgenerate()

            if (titles == null || prices == null)
            {
                // Handle missing session data
                Response.Write("No data to generate invoice.");
                return;
            }

            Document doc = new Document(PageSize.A4, 36, 36, 54, 36);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
            doc.Add(new Paragraph("ASSSK Education Society", titleFont));
            doc.Add(new Paragraph("Invoice Number: " + invoiceNumber));
            doc.Add(new Paragraph("Date: " + invoiceDate.ToString("dd-MM-yyyy")));
            doc.Add(new Paragraph(" "));

            var subFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            doc.Add(new Paragraph("Billed To:", subFont));
            doc.Add(new Paragraph(customerName));
            doc.Add(new Paragraph(customerEmail));
            doc.Add(new Paragraph(" "));

            PdfPTable table = new PdfPTable(2); // Only Product Name and Price
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 70, 30 }); // Adjusted for 2 columns

            AddCell(table, "Product Name", true);
            AddCell(table, "Price", true);

            double total = 0;
            for (int i = 0; i < titles.Count; i++)
            {
                string name = titles[i];
                double price = prices[i];

                AddCell(table, name);
                AddCell(table, "₹" + price.ToString("F2"));

                total += price;
            }

            // Total row
            PdfPCell emptyCell = new PdfPCell(new Phrase("Total"));
            emptyCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            emptyCell.Border = Rectangle.TOP_BORDER;
            emptyCell.Colspan = 1;
            table.AddCell(emptyCell);

            PdfPCell totalCell = new PdfPCell(new Phrase("₹" + total.ToString("F2")));
            totalCell.HorizontalAlignment = Element.ALIGN_RIGHT;
            totalCell.Border = Rectangle.TOP_BORDER;
            table.AddCell(totalCell);

            doc.Add(table);
            doc.Close();
            writer.Close();

            byte[] bytes = memoryStream.ToArray();
            memoryStream.Close();

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=Invoice_" + invoiceNumber + ".pdf");
            // Or to auto-open:
            // Response.AddHeader("Content-Disposition", "inline; filename=Invoice_" + invoiceNumber + ".pdf");

            Response.Buffer = true;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.BinaryWrite(bytes);
            Response.End();
        }


        private void AddCell(PdfPTable table, string text, bool isHeader = false)
        {
            Font font = isHeader ? FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12) :
                                   FontFactory.GetFont(FontFactory.HELVETICA, 11);

            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Padding = 5;
            table.AddCell(cell);
        }

        private string GenerateInvoiceNumber()
        {
            // 🔹 Generate a unique 10-digit invoice number
            string timePart = DateTime.Now.Ticks.ToString().Substring(10, 6);
            string randomPart = new Random().Next(1000, 9999).ToString();
            return timePart + randomPart;
        }
    }

}