using CsvHelper;
using System;
using System.Configuration;
using System.Formats.Asn1;
using System.Globalization;
using System.Reflection;
using TestAssessmentSavenko.Database;
using TestAssessmentSavenko.Models;
using TestAssessmentSavenko.Parsers;
using TestAssessmentSavenko.Validators;

namespace TestAssessmentSavenko
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            
            IParser<TaxiOrder> parser  = new CsvParser<TaxiOrder>();
            IValidator<TaxiOrder> validator = new TaxiOrderValidator();
            IInserter<TaxiOrder> inserter = new TaxiOrderInserter(conn);

            var parsedData = parser.ParseFile("Resources/sample-cab-data.csv", typeof(TaxiOrderMap));
            var validData = validator.FilterValid(parsedData, true) ?? new List<TaxiOrder>();
            inserter.Insert(validData);
        }
    }
}