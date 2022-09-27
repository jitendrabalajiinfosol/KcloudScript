using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KcloudScript.Service
{
    public interface ICsvParserService
    {
        Task<List<T>> ReadCsvFile<T>(string filePath);
        Task<bool> WriteCsvFile<T>(List<T> obj, string filePath);
    }

    public class CsvParserService : ICsvParserService
    {
        public async Task<List<T>> ReadCsvFile<T>(string filePath)
        {
            if (!File.Exists(filePath)) 
            { 
                throw new Exception($"File does not exist on {filePath}."); 
            }

            List<T> ReturnContents = new List<T>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
            using (TextReader fileReader = File.OpenText(filePath))
            {
                fileReader.ReadLine();
                using (CsvReader ReadCsv = new CsvReader(fileReader, config))
                {
                    var data = ReadCsv.GetRecords<T>();
                    ReturnContents.AddRange(data);
                }
            }
            return ReturnContents;
        }

        public async Task<bool> WriteCsvFile<T>(List<T> obj, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteHeader<T>();
                    csv.NextRecord();
                    foreach (var record in obj)
                    {
                        csv.WriteRecord(record);
                        csv.NextRecord();
                    }
                }
            }
            return true;
        }
    }
}
