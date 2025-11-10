using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessmentSavenko.Models;

namespace TestAssessmentSavenko.Parsers
{
    internal class CsvParser<T> : IParser<T>
    {
        public CsvParser() { }

        public IEnumerable<T> Parse(string value, Type? classMapType = null)
        {
            if (string.IsNullOrWhiteSpace(value))
                return Enumerable.Empty<T>();

            using (var reader = new StringReader(value))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                if (classMapType != null) 
                    csv.Context.RegisterClassMap((ClassMap<T>)Activator.CreateInstance(classMapType)!);

                return csv.GetRecords<T>().ToList();
            }
        }

        public IEnumerable<T> ParseFile(string path, Type? classMapType = null)
        {
            using (var reader = new StreamReader(path))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                if (classMapType != null)
                    csv.Context.RegisterClassMap((ClassMap<T>)Activator.CreateInstance(classMapType)!);

                return csv.GetRecords<T>().ToList();
            }
        }
    }
}
