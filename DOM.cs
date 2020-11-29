using System;
using System.Xml;
using System.Xml.Xsl;

namespace Lab2
{
    public class DOM : ISearchAlgoritm
    {
        public XmlDocument Search(Book book, XmlDocument xDoc)
        {

            XmlDocument xDocRes = new XmlDocument();
            XmlElement root = xDocRes.CreateElement("library");

            XmlElement xRoot = xDoc.DocumentElement;
            foreach(XmlNode xnode in xRoot)
            {
                string title = "", author = "", genre = "", language = "";
                int date = 0, pages = 0;
                foreach (XmlAttribute attribute in xnode.Attributes)
                {
                    switch (attribute.Name)
                    {
                        case "title":
                            if (attribute.Value == book.title || book.title == "")
                                title = attribute.Value;
                            break;
                        case "author":
                            if (attribute.Value == book.author || book.author == "")
                                author = attribute.Value;
                            break;
                        case "genre":
                            if (attribute.Value.Contains(book.genre) || book.genre == "")
                                genre = attribute.Value;
                            break;
                        case "language":
                            if (attribute.Value == book.language || book.language == "")
                                language = attribute.Value;
                            break;
                        case "date":
                            if (int.Parse(attribute.Value) >= book.dateFrom && int.Parse(attribute.Value) <= book.dateTo)
                                date = int.Parse(attribute.Value);
                            break;
                        case "pages":
                            if (int.Parse(attribute.Value) >= book.pagesFrom && int.Parse(attribute.Value) <= book.pagesTo)
                                pages = int.Parse(attribute.Value);
                            break;
                        default:
                            break;
                    }
                }
                if(title != "" && author != "" && genre != "" && language != "" && date != 0 && pages != 0)
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
            xDocRes.AppendChild(root);

            return xDocRes;
        }
    }
}




//XmlReader xml = XmlReader.Create();

//XmlDocument doc = new XmlDocument();
//XmlElement root = doc.CreateElement("root");
//root.SetAttribute("name", "value");
//XmlElement child = doc.CreateElement("child");
//child.InnerText = "text node";
//root.AppendChild(child);
//doc.AppendChild(root);







            //XDocument document = new XDocument();

            //// Create root elem
            //XElement STable = new XElement("table");

            //// create first attributes
            //XAttribute SRows = new XAttribute("Rows", Rows);
            //XAttribute SCols = new XAttribute("Cols", Cols);
            //STable.Add(SRows);
            //STable.Add(SCols);

            //foreach (var cell in data)
            //{
            //    if (IsEmpty(cell.Value))//go to next cell if this cell is empty
            //        continue;
            //    XElement SCell = new XElement("cell");

            //    SCell.Add(new XAttribute("Name", cell.Key));//Save the name of cell

            //    if (cell.Value.IOnCells != null && cell.Value.IOnCells.Count != 0)
            //    {//Save IOnCells list
            //        XElement SIOnCells = new XElement("IOnCells");

            //        foreach (var subCell in cell.Value.IOnCells)
            //        {
            //            SIOnCells.Add(new XElement(subCell.Name));
            //        }
            //        SCell.Add(SIOnCells);
            //    }
            //    if (cell.Value.CellsOnMe.Count != 0)
            //    {//save CellsOnMe list
            //        XElement SCellsOnMe = new XElement("CellsOnMe");
            //        foreach (var subCell in cell.Value.CellsOnMe)
            //        {
            //            SCellsOnMe.Add(new XElement(subCell.Name));
            //        }
            //        SCell.Add(SCellsOnMe);
            //    }

            //    SCell.Add(new XAttribute("Expression", cell.Value.Expression));//Save attributes of cell
            //    SCell.Add(new XAttribute("Text", cell.Value.Text));
            //    SCell.Add(new XAttribute("Result", cell.Value.Result));

            //    STable.Add(SCell);
            //}

            //document.Add(STable);
            //document.Save(dialog.Filename);
       