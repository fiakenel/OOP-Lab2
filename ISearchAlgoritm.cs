using System;
using System.Xml;

namespace Lab2
{
    public interface ISearchAlgoritm
    {
        XmlDocument Search(Book book, XmlDocument xDoc);
    }
}
