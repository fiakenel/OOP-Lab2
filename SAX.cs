using System;
using System.IO;
using System.Xml;

namespace Lab2
{
    public class SAX : ISearchAlgoritm
    {
        public XmlDocument Search(Book book, XmlDocument xDoc)
        {
            MemoryStream xmlStream = new MemoryStream();
            xDoc.Save(xmlStream);
            xmlStream.Flush();
            xmlStream.Position = 0;

            XmlTextReader reader = new XmlTextReader(xmlStream);

            XmlDocument xDocRes = new XmlDocument();
            XmlElement root = xDocRes.CreateElement("library");

            while (reader.Read())
            {
                string title = "", author = "", genre = "", language = "";
                int date = 0, pages = 0;
                while (reader.MoveToNextAttribute())
                {
                    switch (reader.Name)
                    {
                        case "title":
                            if (reader.Value == book.title || book.title == "")
                                title = reader.Value;
                            break;
                        case "author":
                            if (reader.Value == book.author || book.author == "")
                                author = reader.Value;
                            break;
                        case "genre":
                            if (reader.Value.Contains(book.genre) || book.genre == "")
                                genre = reader.Value;
                            break;
                        case "language":
                            if (reader.Value == book.language || book.language == "")
                                language = reader.Value;
                            break;
                        case "date":
                            if (int.Parse(reader.Value) >= book.dateFrom && int.Parse(reader.Value) <= book.dateTo)
                                date = int.Parse(reader.Value);
                            break;
                        case "pages":
                            if (int.Parse(reader.Value) >= book.pagesFrom && int.Parse(reader.Value) <= book.pagesTo)
                                pages = int.Parse(reader.Value);
                            break;
                        default:
                            break;
                    }
                }
                if (title != "" && author != "" && genre != "" && language != "" && date != 0 && pages != 0)
                {
                    XmlElement elem = xDocRes.CreateElement("book");
                    elem.SetAttribute("title", title);
                    elem.SetAttribute("author", author);
                    elem.SetAttribute("genre", genre);
                    elem.SetAttribute("language", language);
                    elem.SetAttribute("date", date.ToString());
                    elem.SetAttribute("pages", pages.ToString());

                    root.AppendChild(elem);
                }


            }
            reader.Close();

            xDocRes.AppendChild(root);
            return xDocRes;
        }
    }
}
