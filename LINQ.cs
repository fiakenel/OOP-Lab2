using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Lab2
{
    public class LINQ : ISearchAlgoritm
    {
        public XmlDocument Search(Book book, XmlDocument xDoc)
        {
            MemoryStream xmlStream = new MemoryStream();
            xDoc.Save(xmlStream);
            xmlStream.Flush();
            xmlStream.Position = 0;

            XDocument doc = XDocument.Load(xmlStream);

            var result = from obj in doc.Descendants("book")
                         where 
                         (obj.Attribute("title").Value == book.title || book.title == "") &&
                         (obj.Attribute("author").Value == book.author || book.author == "") &&
                         (obj.Attribute("genre").Value.Contains(book.genre) || book.genre == "") &&
                         (obj.Attribute("language").Value == book.language || book.language == "") &&
                         int.Parse(obj.Attribute("date").Value) >= book.dateFrom &&
                         int.Parse(obj.Attribute("date").Value) <= book.dateTo &&
                         int.Parse(obj.Attribute("pages").Value) >= book.pagesFrom &&
                         int.Parse(obj.Attribute("pages").Value) <= book.pagesTo
                         select new
                         {
                             title = (string)obj.Attribute("title"),
                             author = (string)obj.Attribute("author"),
                             genre = (string)obj.Attribute("genre"),
                             language = (string)obj.Attribute("language"),
                             date = (string)obj.Attribute("date"),
                             pages = (string)obj.Attribute("pages")
                         };

            XmlDocument xDocRes = new XmlDocument();
            XmlElement root = xDocRes.CreateElement("library");
            foreach (var n in result)
            {
                XmlElement elem = xDocRes.CreateElement("book");
                elem.SetAttribute("title", n.title);
                elem.SetAttribute("author", n.author);
                elem.SetAttribute("genre", n.genre);
                elem.SetAttribute("language", n.language);
                elem.SetAttribute("date", n.date);
                elem.SetAttribute("pages", n.pages);

                root.AppendChild(elem);
            }
            xDocRes.AppendChild(root);
            return xDocRes;
        }
    }
}
