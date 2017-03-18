using Sgml;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace NikkiSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                using (var sgml = new SgmlReader { Href = "https://miraclenikki.gamerch.com/%E3%83%98%E3%82%A2%E3%82%B9%E3%82%BF%E3%82%A4%E3%83%AB" })
                {
                    var doc = new XmlDocument();
                    doc.Load(sgml);

                    foreach (XmlElement elem in doc.GetElementsByTagName("table").Cast<XmlElement>())
                    {
                        if (elem.GetAttribute("id").StartsWith("ui_wikidb_table_"))
                        {
                            Console.WriteLine(elem.GetAttribute("id"));
                        }

                        foreach (XmlElement item in elem.GetElementsByTagName("tr"))
                        {

                        }
                    }
                }
            }
        }
    }
}
