using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using TutorialDemos.PDFConsole.Models;

namespace TutorialDemos.PDFConsole.Building;

public class MakePDF
{
    public static readonly PdfNumber PORTRAIT = new PdfNumber(0);
    public static readonly PdfNumber LANDSCAPE = new PdfNumber(90);
    public MakePDF()
    {

    }

    public static void Build()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        path = System.IO.Path.Combine(path, "demo_test.pdf");
        PdfWriter writer = new(path);

        //BuildPdfModule(writer);
        BuildTrialImage(writer);
    }



    public static void InMemoryBuiltPDF()
    {
        var stream = new MemoryStream();
        var writer = new PdfWriter(stream);
        var result = BuildPdfModule(writer, stream);

        Utilities.SendEmail("Testing attachment", result);
    }

    public static void BuildFromdynamicData()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        path = System.IO.Path.Combine(path, "demo.pdf");
        PdfWriter writer = new(path);

        var obj = new
        {
            name = "john",
            last = "tuza"
        };

        var objList = new List<dynamic>
        {
            obj,
            obj,
            obj,
            obj
        };


        PdfDocument pdf = new(writer);
        Document document = new(pdf);

        Paragraph newline = new(new Text("\n"));

        Paragraph header = new Paragraph("HEADER")
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFontSize(20);

        Paragraph subheader = new Paragraph("SUB HEADER")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(15);

        LineSeparator ls = new(new SolidLine());


        byte[] bytes = Convert.FromBase64String(Utilities.GetXenteLogoBase64Image());
        Image img = new Image(ImageDataFactory
            .Create(bytes))
            .SetHeight(25)
            .SetTextAlignment(TextAlignment.RIGHT);

        var counter = obj.GetPropertiesCount(out List<System.Reflection.PropertyInfo> properties);

        Table table = new(counter, true);
        table.SetMarginTop(10);


        foreach (var headerTitle in properties)
        {
            Cell headerCell = new Cell(1, 1)
                .SetBackgroundColor(ColorConstants.GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(headerTitle.Name));


            table.AddCell(headerCell);
        }
        foreach (var item in objList)
        {

            var itemProperties = ((object)item)
                .GetType()
                .GetProperties();


            foreach (var propItem in itemProperties)
            {
                var value = propItem.GetValue(item);

                Cell itemCell = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph(value?.ToString() ?? ""));


                table.AddCell(itemCell);
            }


        }
    }

    private static byte[]? BuildPdfModule(PdfWriter writer, MemoryStream stream = null)
    {
        PdfDocument pdf = new(writer);
        Document document = new(pdf);

        Paragraph newline = new(new Text("\n"));

        Paragraph header = new Paragraph("HEADER")
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFontSize(20);

        Paragraph subheader = new Paragraph("SUB HEADER")
            .SetTextAlignment(TextAlignment.CENTER)
            .SetFontSize(15);

        LineSeparator ls = new(new SolidLine());

        byte[] bytes = Convert.FromBase64String(Utilities.GetXenteLogoBase64Image());
        Image img = new Image(ImageDataFactory
            .Create(bytes))
            .SetHeight(25)
            .SetTextAlignment(TextAlignment.RIGHT);

        var student = new Student();

        var counter = student.GetPropertiesCount(out List<System.Reflection.PropertyInfo> properties);

        Table table = new(counter, true);
        table.SetMarginTop(10);


        foreach (var headerTitle in properties)
        {
            Cell headerCell = new Cell(1, 1)
                .SetBackgroundColor(ColorConstants.GRAY)
                .SetTextAlignment(TextAlignment.CENTER)
                .Add(new Paragraph(headerTitle.Name));


            table.AddCell(headerCell);
        }

        foreach (var studentItem in Student.GetStudents())
        {

            studentItem.GetPropertiesCount(out properties);

            foreach (var propItem in properties)
            {
                var value = propItem.GetValue(studentItem);

                Cell itemCell = new Cell(1, 1)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .Add(new Paragraph(value?.ToString() ?? ""));


                table.AddCell(itemCell);
            }


        }


        Link link = new("click here",
            PdfAction.CreateURI("https://www.google.com"));

        Paragraph hyperLink = new Paragraph("Please ")
           .Add(link.SetBold().SetUnderline()
           .SetItalic().SetFontColor(ColorConstants.BLUE))
           .Add(" to go www.google.com.");


        // Page numbers
        int n = pdf.GetNumberOfPages();
        for (int i = 1; i <= n; i++)
        {
            document.ShowTextAligned(new Paragraph(String
               .Format("page" + i + " of " + n)),
                559, 806, i, TextAlignment.RIGHT,
                VerticalAlignment.TOP, 0);
        }

        document.Add(header);
        document.Add(subheader);
        document.Add(newline);
        document.Add(ls);

        document.Add(newline);
        document.Add(img);
        document.Add(newline);
        document.Add(table);

        document.Add(newline);
        document.Add(hyperLink);

        document.Close();

        if (stream != null)
            return stream.ToArray();

        return null;
    }


    private static void BuildTrialImage(PdfWriter writer)
    {
        PdfDocument pdf = new(writer);
        var pageSize = PageSize.A4.Rotate();
        Document document = new(pdf, pageSize);

        document.SetBottomMargin(35);
        document.SetTopMargin(140);


        byte[] bytes = Convert.FromBase64String(Utilities.GetXenteWaterMarkBase64Image());
        var imageData = ImageDataFactory.Create(bytes);

        pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextHeaderEventHandler(document));
        pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new TextFooterEventHandler(document));
        pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new BackgroundEventHandler(imageData));

        bytes = Convert.FromBase64String(Utilities.GetXenteLogoBase64Image());
        Image img = new Image(ImageDataFactory
            .Create(bytes))
            .SetWidth(150)
            .SetFixedPosition(650, 495)
            .SetTextAlignment(TextAlignment.RIGHT);

        var student = new Student();

        var counter = student.GetPropertiesCount(out List<System.Reflection.PropertyInfo> properties);

        Table table = new Table(counter, true);

        foreach (var headerTitle in properties)
        {
            Cell headerCell = new Cell(1, 1);

            var paragraph = new Paragraph(headerTitle.Name);
            paragraph.SetBold();
            paragraph.SetFontSize(9);
            headerCell.Add(paragraph);

            headerCell.SetBorderRight(Border.NO_BORDER);
            headerCell.SetBorderLeft(Border.NO_BORDER);
            headerCell.SetBorderTop(Border.NO_BORDER);

            table.AddCell(headerCell);
        }

        foreach (var studentItem in Student.GetStudents())
        {

            studentItem.GetPropertiesCount(out properties);

            foreach (var propItem in properties)
            {
                var studentValue = propItem.GetValue(studentItem);

                Cell itemCell = new Cell(1, 1)
                    .Add(new Paragraph(studentValue?.ToString() ?? "")
                    .SetFontSize(9));

                itemCell.SetBorderRight(Border.NO_BORDER);
                itemCell.SetBorderLeft(Border.NO_BORDER);

                table.AddCell(itemCell);
            }


        }


        Link link = new("click here",
            PdfAction.CreateURI("https://www.google.com"));

        Paragraph hyperLink = new Paragraph("Please ")
           .Add(link.SetBold().SetUnderline()
           .SetItalic().SetFontColor(ColorConstants.BLUE))
           .Add(" to go www.google.com.");

        Paragraph newline = new Paragraph(new Text("\n"));

        document.Add(newline);
        document.Add(table);
        document.Close();
    }

    private class PageRotationEventHandler : IEventHandler
    {
        private PdfNumber rotation = LANDSCAPE;

        public void SetRotation(PdfNumber orientation)
        {
            this.rotation = orientation;
        }

        public void HandleEvent(Event currentEvent)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            docEvent.GetPage().Put(PdfName.Rotate, rotation);
        }
    }
    private class TextFooterEventHandler : IEventHandler
    {
        protected Document doc;

        public TextFooterEventHandler(Document doc)
        {
            this.doc = doc;
        }


        public void HandleEvent(Event currentEvent)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            Rectangle pageSize = docEvent.GetPage().GetPageSize();
            PdfFont font = null;

            try
            {
                font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_OBLIQUE);
            }
            catch (IOException e)
            {
                Console.Error.WriteLine(e.Message);
            }


            PdfPage page = docEvent.GetPage();
            int pageNumber = docEvent.GetDocument().GetPageNumber(page);

            float coordX = ((pageSize.GetLeft() + doc.GetLeftMargin())
                             + (pageSize.GetRight() - doc.GetRightMargin())) / 2;

            Paragraph p = new Paragraph()
                   .Add("Page ")
                   .Add(pageNumber.ToString())
                   .Add(" of ")
                   .Add(docEvent.GetDocument().GetNumberOfPages().ToString());

            Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);
            canvas
                .SetFont(font)
                .SetFontSize(9)
                .ShowTextAligned(p, 805, 18, TextAlignment.RIGHT)
                .ShowTextAligned("Support contacts:  +256 753 188994 | +256 778 324257 | support@xente.co: footer", coordX, 15, TextAlignment.CENTER)
                .Close();
        }
    }

    private class TextHeaderEventHandler : IEventHandler
    {
        protected Document doc;

        public TextHeaderEventHandler(Document doc)
        {
            this.doc = doc;
        }

        public void HandleEvent(Event currentEvent)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            Rectangle pageSize = docEvent.GetPage().GetPageSize();
            PdfFont font = null;
            try
            {
                font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            }
            catch (IOException e)
            {

                // Such an exception isn't expected to occur,
                // because helvetica is one of standard fonts
                Console.Error.WriteLine(e.Message);
            }

            Table table = new Table(2, true);

            table.SetMarginLeft(36);
            table.SetMarginTop(5);

            var rightCell = new Cell(1, 1)
                .SetBorder(Border.NO_BORDER)
                .SetFont(font)
                .SetFontSize(9);
            rightCell.SetPaddingTop(10);
            rightCell.Add(new Paragraph("Transaction Report")
                .SetFontSize(14)
                .SetBold());
            rightCell.Add(new Paragraph(new Text("\n")));

            PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var companyNameParagraph = new Paragraph();
            var companyTitle = new Text("Company name: ").SetFont(boldFont);
            companyNameParagraph.Add(companyTitle)
                .Add(" TEST BUSINESS");

            rightCell.Add(companyNameParagraph);
            rightCell.Add(new Paragraph(new Text("Account name ").SetFont(boldFont))
                .Add("Account name"));

            rightCell.Add(new Paragraph(new Text("Currency ").SetFont(boldFont))
                .Add("UGX"));

            rightCell.Add(new Paragraph(new Text("Period ").SetFont(boldFont))
                .Add("today"));

            rightCell.Add(new Paragraph(new Text("Report date ").SetFont(boldFont))
                .Add("This date"));

            table.AddCell(rightCell);


            var leftCell = new Cell(1, 1)
                .SetBorder(Border.NO_BORDER);
            var bytes = Convert.FromBase64String(Utilities.GetXenteLogoBase64Image());
            Image img = new Image(ImageDataFactory
            .Create(bytes))
            .SetWidth(150)
            .SetTextAlignment(TextAlignment.RIGHT);

            leftCell.Add(img);
            leftCell.SetPaddingTop(40);
            leftCell.SetPaddingLeft(210);
            leftCell.SetHorizontalAlignment(HorizontalAlignment.RIGHT);
            table.AddCell(leftCell);



            Canvas canvas = new Canvas(docEvent.GetPage(), pageSize);
            canvas
                .Add(table)
                .Close();
        }
    }
    protected class BackgroundEventHandler : IEventHandler
    {
        protected ImageData imageData;

        public BackgroundEventHandler(ImageData imageData)
        {
            this.imageData = imageData;
        }
        public void HandleEvent(Event currentEvent)
        {
            PdfDocumentEvent docEvent = (PdfDocumentEvent)currentEvent;
            PdfDocument pdfDoc = docEvent.GetDocument();
            PdfPage page = docEvent.GetPage();

            PdfCanvas canvas = new PdfCanvas(page.NewContentStreamBefore(),
                page.GetResources(), pdfDoc);

            Rectangle rect = new Rectangle(200, -25, 650, 290);

            canvas.AddImageFittedIntoRectangle(imageData, rect, false);
            new Canvas(canvas, rect, false);
        }
    }
}
