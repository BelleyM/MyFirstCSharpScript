using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        private static string FILE_PATTERN = "^([a-zA-Z]):[\\\\/]((?:[^<>:\"\\\\/\\|\\?\\*]+[\\\\/])*)([^<>:\"\\\\/\\|\\?\\*]+)\\.csv$";
        static int Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Numero di parametri non corretto");
                return -1;
            }
            string filePath = args[0];
            Regex regex = new Regex(FILE_PATTERN, RegexOptions.IgnoreCase);

            if (!regex.Match(filePath).Success)
            {
                Console.WriteLine("File path does not match regualar expression. Please check it and try again.");
                return -1;
            }
            var index = Int32.Parse(args[1]);
            string key = args[2];
            try
            {
                using (var reader = new StreamReader(filePath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Configuration.HasHeaderRecord = false;
                var records = csv.GetRecords<CSVModel>();
                foreach (var r in records)
                {
                    var found = FindKey(index, key, r);
                   if ( found != null)
                    {
                        WriteCSVLine(found);
                        return 0;
                    }
                }
            }
            }
            catch (System.IO.FileNotFoundException e)
            {
                Console.WriteLine("File not found.");
                return -1;
            }
            Console.WriteLine("Value: " + key + " in column: " + index + " not found!");
            return 1;
        }

        private static CSVModel FindKey(int index, string key, CSVModel r)
        {
            switch (index)
            {
                case 0:
                    if (key.Equals(r.Id))
                    {
                       return r;
                    }
                    break;
                case 1:
                    if (key.Equals(r.FamName))
                    {
                        return r;
                    }
                    break;
                case 2:
                    if (key.Equals(r.Name))
                    {
                        return r;
                    }
                    break;
                case 3:
                    if (key.Equals(r.BirtDate))
                    {
                        return r;
                    }
                    break;
                default:
                    Console.WriteLine("Row number outbound!");
                    return null;
            }
            return null;
        }

        private static void WriteCSVLine(CSVModel r)
        {
            Console.WriteLine(r.Id+","+ r.FamName + "," + r.Name + "," +  r.BirtDate) ;          
        }
    }
    class CSVModel
    {
        [Index(0)]
        public int Id { get; set; }
        [Index(1)] 
        public string Name { get; set; }
        [Index(2)]
        public string FamName { get; set; }
        [Index(3)]
        public string BirtDate { get; set; }
    }
}
