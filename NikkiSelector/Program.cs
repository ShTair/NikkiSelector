using CsvHelper;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NikkiSelector
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, Item> items;
            using (var reader = File.OpenText(args[0]))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.RegisterClassMap<Item.Map>();
                csv.Configuration.HasHeaderRecord = false;
                items = csv.GetRecords<Item>().ToDictionary(t => t.Id, t => t);
            }
        }
    }
}
