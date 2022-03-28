using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Action;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using TutorialDemos.PDFConsole.Models;

namespace TutorialDemos.PDFConsole.Building;

public class MakePDF
{
    public MakePDF()
    {

    }

    public static void Build()
    {
        var path = AppDomain.CurrentDomain.BaseDirectory;

        path = Path.Combine(path, "demo.pdf");
        PdfWriter writer = new(path);

        BuildPdfModule(writer);
    }



    public static void InMemoryBuiltPDF()
    {
        var stream = new MemoryStream();
        var writer = new PdfWriter(stream);
        var result = BuildPdfModule(writer, stream);

        var tt = result;
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


        byte[] bytes = Convert.FromBase64String(Utilities.GetBase64Image());
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
}
