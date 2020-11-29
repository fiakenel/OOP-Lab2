using System;
using System.Xml;
using Gtk;
using System.Collections.Generic;
using Lab2;
using System.IO;
using System.Xml.Xsl;

public partial class MainWindow : Gtk.Window
{
    private int maxDate;
    private int minDate;

    private int maxPages;
    private int minPages;


    List<string> titles;
    List<string> authors;
    List<string> genres;
    List<string> languages;

    XmlDocument lastxDoc;

    string path;

    public MainWindow(string path) : base(Gtk.WindowType.Toplevel)
    {
        this.path = path;
        Build();

        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);//load file
        lastxDoc = xDoc;
        OpenData(xDoc);
    }

    private void OpenData(XmlDocument xDoc)
    {
        titles = new List<string>();
        authors = new List<string>();
        genres = new List<string>();
        languages = new List<string>();
        textview.Buffer.Text = "";

        XmlElement xRoot = xDoc.DocumentElement;//main root

        int counter = 0;
        foreach (XmlNode xnode in xRoot)//book
        {
            if (counter == 0)
            {
                counter++;

                maxDate = int.Parse(xnode.Attributes.GetNamedItem("date").Value);
                minDate = maxDate;

                maxPages = int.Parse(xnode.Attributes.GetNamedItem("pages").Value);
                minPages = maxPages;
            }
            foreach (XmlAttribute attribute in xnode.Attributes)//attributes of a book
            {
                switch (attribute.Name)
                {
                    case "title":
                        if (!titles.Contains(attribute.Value))
                            titles.Add(attribute.Value);
                        textview.Buffer.Text += "Назва: ";
                        break;

                    case "author":
                        if (!authors.Contains(attribute.Value))
                            authors.Add(attribute.Value);
                        textview.Buffer.Text += "автор: ";
                        break;
                    case "genre":
                        foreach (string genre in attribute.Value.Split(','))
                        {
                            if (!genres.Contains(genre))
                                genres.Add(genre);
                        }
                        textview.Buffer.Text += "Жанр: ";
                        break;

                    case "language":
                        if (!languages.Contains(attribute.Value))
                            languages.Add(attribute.Value);
                        textview.Buffer.Text += "Мова оригіналу: ";
                        break;

                    case "date":
                        int temp1 = int.Parse(attribute.Value);
                        if (temp1 < minDate)
                            minDate = temp1;
                        else if (temp1 > maxDate)
                            maxDate = temp1;

                        textview.Buffer.Text += "Дата написання: ";
                        break;

                    case "pages":
                        int temp2 = int.Parse(attribute.Value);
                        if (temp2 < minPages)
                            minPages = temp2;
                        else if (temp2 > maxPages)
                            maxPages = temp2;

                        textview.Buffer.Text += "Кількість сторінок: ";
                        break;

                    default:
                        throw new Exception();
                }
                textview.Buffer.Text += attribute.Value + "\n";
            }
            textview.Buffer.Text += "\n";
        }

        //making upper and lower borders for spinbuttons
        spinbuttonDateTo.Adjustment.Upper = maxDate;
        spinbuttonDateFrom.Adjustment.Upper = maxDate;
        spinbuttonDateTo.Adjustment.Value = maxDate;

        spinbuttonDateTo.Adjustment.Lower = minDate;
        spinbuttonDateFrom.Adjustment.Lower = minDate;
        spinbuttonDateFrom.Adjustment.Value = minDate;

        spinbuttonPagesTo.Adjustment.Upper = maxPages;
        spinbuttonPagesFrom.Adjustment.Upper = maxPages;
        spinbuttonPagesTo.Adjustment.Value = maxPages;

        spinbuttonPagesTo.Adjustment.Lower = minPages;
        spinbuttonPagesFrom.Adjustment.Lower = minPages;
        spinbuttonPagesFrom.Adjustment.Value = minPages;



        //filling comboboxes
        foreach (string title in titles)
            comboboxTitle.AppendText(title);
        foreach (string genre in genres)
            comboboxGenre.AppendText(genre);
        foreach (string language in languages)
            comboboxLanguage.AppendText(language);
        foreach (string author in authors)
            comboboxAuthor.AppendText(author);
    }

    private void OnClearClicked(object sender, EventArgs e)
    {
        comboboxTitle.Model = new ListStore(typeof(string), typeof(string));
        comboboxGenre.Model = new ListStore(typeof(string), typeof(string));
        comboboxLanguage.Model = new ListStore(typeof(string), typeof(string));
        comboboxAuthor.Model = new ListStore(typeof(string), typeof(string));


        XmlDocument xDoc = new XmlDocument();
        xDoc.Load(path);//load file
        OpenData(xDoc);
        lastxDoc = xDoc;
    }

    private void OnSearchClicked(object sender, EventArgs e)
    {
        FindBooks();
    }

    private void FindBooks()
    {
        if (spinbuttonDateFrom.ValueAsInt > spinbuttonDateTo.ValueAsInt ||
         spinbuttonPagesFrom.ValueAsInt > spinbuttonPagesTo.ValueAsInt)
        {
            MessageDialog dialog = new MessageDialog(this, DialogFlags.Modal,
                MessageType.Error, ButtonsType.Ok, "gddhfdh");
            dialog.Text = "Некоректно введені дата або кількість сторінок";
            dialog.Run();
            dialog.Destroy();
            return;
        }
        Book book = new Book
        {
            title = comboboxTitle.ActiveText == null ? "" : comboboxTitle.ActiveText,
            author = comboboxAuthor.ActiveText == null ? "" : comboboxAuthor.ActiveText,
            genre = comboboxGenre.ActiveText == null ? "" : comboboxGenre.ActiveText,
            language = comboboxLanguage.ActiveText == null ? "" : comboboxLanguage.ActiveText,
            dateFrom = spinbuttonDateFrom.ValueAsInt,
            dateTo = spinbuttonDateTo.ValueAsInt,
            pagesFrom = spinbuttonPagesFrom.ValueAsInt,
            pagesTo = spinbuttonPagesTo.ValueAsInt
        };


        ISearchAlgoritm algoritm;
        if (radiobuttonDOM.Active)
            algoritm = new DOM();
        else if (radiobuttonSAX.Active)
            algoritm = new SAX();
        else
            algoritm = new LINQ();

        XmlDocument xDoc = algoritm.Search(book, lastxDoc);
        
        comboboxTitle.Model = new ListStore(typeof(string), typeof(string));
        comboboxGenre.Model = new ListStore(typeof(string), typeof(string));
        comboboxLanguage.Model = new ListStore(typeof(string), typeof(string));
        comboboxAuthor.Model = new ListStore(typeof(string), typeof(string));
        OpenData(xDoc);
        lastxDoc = xDoc;
    }

    private void BtnConvertActivated(object sender, EventArgs e)
    {
        ConvertToHTML();
    }

    private void ConvertToHTML()
    {
        MemoryStream xmlStream = new MemoryStream();
        lastxDoc.Save(xmlStream);
        xmlStream.Flush();
        xmlStream.Position = 0;

        XmlReader reader = XmlReader.Create(xmlStream);
        reader.MoveToContent();

        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;
        settings.IndentChars = "\t";
        XmlWriter writer = XmlWriter.Create("/home/maksym/Documents/MonoDevelop/Lab2/result.html", settings);

        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load("/home/maksym/Documents/MonoDevelop/Lab2/style.xslt");
        xslt.Transform(reader, writer);

        reader.Close();
        writer.Close();
    }

    private void AboutAuthorActivated(object sender, EventArgs e)
    {
        AboutDialog about = new AboutDialog()
        {
            ProgramName = "Пошук по базі даних",
            Authors = new string[] { "Карапуд Максим К-26" },
            Version = "Версія 1.0.0"
        };
        about.Run();
        about.Destroy();
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }
}

