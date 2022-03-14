using System.Linq;
using Core.Interfaces;
using Infrastructure.Data;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Infrastructure.Services
{
    public class PdfService : IPdfService
    {
        private readonly HappyKidsContext _context;
        public PdfService(HappyKidsContext context)
        {
            _context = context;
        }

        public void GeneratePdfForGeneralCardSlip(int orderNo, decimal amount, string firstName, string lastName)
        {
            var accounts = _context.Accounts.ToList();

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            PdfDocument document = new PdfDocument();

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Arial", 15);

            gfx.DrawString("Order No: " + orderNo.ToString(), new XFont("Arial", 30, XFontStyle.Bold), XBrushes.Red, new XPoint(170, 70));
            gfx.DrawLine(new XPen(XColor.FromArgb(50, 30, 200)), new XPoint(50, 100), new XPoint(550, 100));

            gfx.DrawString("Customer: " + lastName + ", " + firstName,
            new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(50, 130));

            gfx.DrawString("Amount: " + amount.ToString(),
            new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black, new XPoint(50, 160));

            gfx.DrawString("Please pay specified amount on one of the following accounts:",
            new XFont("Arial", 10, XFontStyle.Bold), XBrushes.Red, new XPoint(50, 195));

            int currentYposition_values = 230;

            for (int i = 0; i < accounts.Count(); i++)
            {
                gfx.DrawString(accounts[i].BankName + ", " + accounts[i].IBAN,
                    new XFont("Arial", 15, XFontStyle.Bold), XBrushes.Black,
                    new XPoint(50, currentYposition_values));

                currentYposition_values += 30;
            }
            
            document.Save("C:\\Users\\petar\\source\\repos\\" + orderNo.ToString() + ".pdf");
        }
    }
}