using CsvHelper;
using NikkiSelector.Models;
using Sgml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace NikkiSelector
{
    class Program
    {
        private static Dictionary<int, Item> Items;

        static void Main(string[] args)
        {
            LoadCsv(args[0]);

            Load(0, "1_ヘアスタイル", "https://miraclenikki.gamerch.com/%E3%83%98%E3%82%A2%E3%82%B9%E3%82%BF%E3%82%A4%E3%83%AB");
            Load(10000, "2_ドレス", "https://miraclenikki.gamerch.com/%E3%83%89%E3%83%AC%E3%82%B9");
            Load(20000, "3_コート", "https://miraclenikki.gamerch.com/%E3%82%B3%E3%83%BC%E3%83%88");
            Load(30000, "4_トップス", "https://miraclenikki.gamerch.com/%E3%83%88%E3%83%83%E3%83%97%E3%82%B9");
            //Load("5_ボトムス", "https://miraclenikki.gamerch.com/%E3%83%9C%E3%83%88%E3%83%A0%E3%82%B9");
            //Load("6_靴下", "https://miraclenikki.gamerch.com/%E9%9D%B4%E4%B8%8B");
            //Load("7_シューズ", "https://miraclenikki.gamerch.com/%E3%82%B7%E3%83%A5%E3%83%BC%E3%82%BA");
            //Load("15_メイク", "https://miraclenikki.gamerch.com/%E3%83%A1%E3%82%A4%E3%82%AF");

            //Load("8_ヘアアクセサリー", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E9%A0%AD");
            //Load("9_耳飾り", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E8%80%B3");
            //Load("10_首飾り", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E9%A6%96");
            //Load("11_腕飾り", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E8%85%95");
            //Load("12_手持品", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E6%89%8B");
            //Load("13_腰飾り", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E8%85%B0");
            //Load("14_特殊", "https://miraclenikki.gamerch.com/%E3%82%A2%E3%82%AF%E3%82%BB%E3%82%B5%E3%83%AA%E3%83%BC%E3%83%BB%E7%89%B9%E6%AE%8A");

            SaveCsv(args[0]);
        }

        private static void LoadCsv(string path)
        {
            using (var reader = File.OpenText(path))
            {
                // headerを10行読み飛ばす
                for (int i = 0; i < 9; i++) reader.ReadLine();

                using (var csv = new CsvReader(reader))
                {
                    csv.Configuration.RegisterClassMap<Item.Map>();
                    Items = csv.GetRecords<Item>().ToDictionary(t => t.Id, t => t);
                }
            }
        }

        private static void SaveCsv(string path)
        {
            using (var writer = new StreamWriter(path, false, new UTF8Encoding(true)))
            {
                writer.WriteLine(@"MiracleNikkiJp_items.csv,,,,,,,,,,,,,,,,,
FormatVersion,3,アクセサリーの種類を細分化しました,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,,
このフォーマットでは、アイテムの番号を効率よく管理するために、通しの整理番号を付与しています。,,,,,,,,,,,,,,,,,
整理番号1は必ず11行にします。最後の10行は必ず、種類と名前を－にした「空き領域」にします。,,,,,,,,,,,,,,,,,
整理番号と、番号(アプリ中の番号)と、種類の関係は固定で、変更しないこととします。この固定により整理番号とアイテムの所持情報をリンクすることができ、このファイルの何行目なのかも特定できるようになります。,,,,,,,,,,,,,,,,,
アイテムの種類毎に番号1から10000まで記述可能な書式になっています。全部埋まると90000アイテムです。アクセサリーはまとめて10000までです。,,,,,,,,,,,,,,,,,
種類の一覧は、整理番号89901から記述しています。,,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,,
整理番号,番号(アプリ中の番号),種類,－,名前,ハート数,華麗,シンプル,エレガント,アクティブ,大人,キュート,セクシー,ピュア,ウォーム,クール,タグ (複数は空白区切り),色 (複数は空白区切り)");

                using (var csv = new CsvWriter(writer))
                {
                    csv.Configuration.RegisterClassMap<Item.Map>();
                    csv.Configuration.HasHeaderRecord = false;
                    csv.WriteRecords(Items.Values);
                }
            }
        }

        private static void Load(int bid, string name, string uri)
        {
            var z = new double[] { 0, 2.0 / 3, 3.0 / 3, 4.0 / 3 };

            var vs = new double[] {
                z[0]/* 華麗 */,
                z[0]/* シンプル */,

                z[0]/* エレガント */,
                z[0]/* アクティブ */,

                z[0]/* 大人 */,
                z[0]/* キュート */,

                z[0]/* セクシー */,
                z[0]/* ピュア */,

                z[0]/* ウォーム */,
                z[0]/* クール */,
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
                                case "X": return 0;
                                case "D": return 2;
                                case "C": return 3;
                                case "B": return 4;
                                case "A": return 5;
                                case "S": return 7;
                                case "SS": return 9;
                                case "SSS": return 11;
                                case "SSSS": return 13;
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
                                stream.WriteLine("score," + string.Join(",", elem.GetElementsByTagName("th").Cast<XmlElement>().Select(t => t.InnerText)));
                            }

                            foreach (var item in from trs in elem.GetElementsByTagName("tr").Cast<XmlElement>()
                                                 let cs = trs.GetElementsByTagName("td").Cast<XmlElement>().ToList()
                                                 where cs.Count != 0
                                                 select cs.Select(t => t.InnerText).ToList())
                            {
                                var r = item.Skip(4).Take(10).Select(t => cv(t)).ToList();
                                var sc = Enumerable.Range(0, 10).Select(t => r[t] * vs[t]).Sum();
                                stream.WriteLine(sc + "," + string.Join(",", item));

                                var iname = item[2].Replace("（", "(").Replace("）", ")").Replace("(ヘアスタイル)", "").Replace("(トップス)", "");
                                switch (iname)
                                {
                                    case "パールレディ": iname = "パールレディー"; break;
                                }

                                var id = int.Parse(item[1]) + bid;

                                switch (id)
                                {
                                    case 198: continue;
                                }

                                var si = Items[id];
                                si.Kind = item[0];
                                si.Name = iname;
                                si.Rarity = item[3].Substring(1);
                                si.P11 = item[4].ToUpper();
                                si.P12 = item[5].ToUpper();
                                si.P21 = item[6].ToUpper();
                                si.P22 = item[7].ToUpper();
                                si.P31 = item[8].ToUpper();
                                si.P32 = item[9].ToUpper();
                                si.P41 = item[10].ToUpper();
                                si.P42 = item[11].ToUpper();
                                si.P51 = item[12].ToUpper();
                                si.P52 = item[13].ToUpper();
                                si.Tags = (item[14] + " " + item[15]).Trim();
                            }
                        }
                    }
                }
            }
        }
    }
}
