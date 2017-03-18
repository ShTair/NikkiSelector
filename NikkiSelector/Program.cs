using Sgml;
using System.IO;
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
            Load("1_ヘアスタイル", "https://miraclenikki.gamerch.com/%E3%83%98%E3%82%A2%E3%82%B9%E3%82%BF%E3%82%A4%E3%83%AB");
            Load("2_ドレス", "https://miraclenikki.gamerch.com/%E3%83%89%E3%83%AC%E3%82%B9");
            Load("3_コート", "https://miraclenikki.gamerch.com/%E3%82%B3%E3%83%BC%E3%83%88");
            Load("4_トップス", "https://miraclenikki.gamerch.com/%E3%83%88%E3%83%83%E3%83%97%E3%82%B9");
            Load("5_ボトムス", "https://miraclenikki.gamerch.com/%E3%83%9C%E3%83%88%E3%83%A0%E3%82%B9");
            Load("6_靴下", "https://miraclenikki.gamerch.com/%E9%9D%B4%E4%B8%8B");
            Load("7_シューズ", "https://miraclenikki.gamerch.com/%E3%82%B7%E3%83%A5%E3%83%BC%E3%82%BA");
            Load("15_メイク", "https://miraclenikki.gamerch.com/%E3%83%A1%E3%82%A4%E3%82%AF");

            Load("8_ヘアアクセサリー", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E9%A0%AD");
            Load("9_耳飾り", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E8%80%B3");
            Load("10_首飾り", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E9%A6%96");
            Load("11_腕飾り", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E8%85%95");
            Load("12_手持品", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E6%89%8B");
            Load("13_腰飾り", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E8%85%B0");
            Load("14_特殊", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E7%89%B9%E6%AE%8A");
        }

        private static void Load(string name, string uri)
        {
            var vs = new double[] {
                /* 華麗 */0,
                /* シンプル */1.5,
                /* エレガント */0,
                /* アクティブ */1.5,
                /* 大人 */0,
                /* キュート */1.5,
                /* セクシー */0,
                /* ピュア */1.2,
                /* ウォーム */0,
                /* クール */1,
            };

            using (var wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                using (var sgml = new SgmlReader { Href = uri })
                {
                    var doc = new XmlDocument();
                    doc.Load(sgml);

                    bool h = true;
                    using (var stream = new StreamWriter(name + ".csv", false, Encoding.Default))
                    {
                        double cv(string v)
                        {
                            switch (v.Trim())
                            {
                                case "C": return 1;
                                case "B": return 2;
                                case "A": return 3;
                                case "S": return 4;
                                case "SS": return 5;
                            }

                            return 0;
                        }

                        foreach (XmlElement elem in doc.GetElementsByTagName("table").Cast<XmlElement>())
                        {
                            var tid = elem.GetAttribute("id");
                            if (!tid.StartsWith("ui_wikidb_table_")) continue;

                            if (h)
                            {
                                h = false;
                                stream.WriteLine("score" + string.Join(",", elem.GetElementsByTagName("th").Cast<XmlElement>().Select(t => t.InnerText)));
                            }

                            foreach (var item in from trs in elem.GetElementsByTagName("tr").Cast<XmlElement>()
                                                 let cs = trs.GetElementsByTagName("td").Cast<XmlElement>().ToList()
                                                 where cs.Count != 0
                                                 select cs.Select(t => t.InnerText).ToList())
                            {
                                var r = item.Skip(4).Take(10).Select(t => cv(t)).ToList();
                                var sc = Enumerable.Range(0, 10).Select(t => r[t] * vs[t]).Sum();
                                stream.WriteLine(sc + "," + string.Join(",", item));
                            }
                        }
                    }
                }
            }
        }
    }
}
