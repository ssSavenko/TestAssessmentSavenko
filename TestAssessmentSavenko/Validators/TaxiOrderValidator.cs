using CsvHelper;
using System;
using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessmentSavenko.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestAssessmentSavenko.Validators
{
    internal class TaxiOrderValidator : IValidator<TaxiOrder>
    {
        private string reportDir = "duplicates.csv";

        public TaxiOrderValidator() { }
        public TaxiOrderValidator(string reportDir)
        {
            this.reportDir = reportDir;
        }

        public string ReportDir { get => reportDir; set => reportDir = value; }

        public IEnumerable<TaxiOrder> FilterValid(IEnumerable<TaxiOrder> values, bool report = false)
        {
            //TODO Add separate logger for reports
            var tupple = FilterDuplicated(values);
            if (report)
                SaveReported(tupple?.Item2 ?? new List<TaxiOrder>());
            return tupple?.Item1 ?? new List<TaxiOrder>();
        }

        public bool Validate(TaxiOrder value, bool report = false)
        {
            //TODO Add separate logger for reports
            return true;
        }

        public bool Validate(IEnumerable<TaxiOrder> values, bool report = false)
        {
            //TODO Add separate logger for reports
            var tupple = FilterDuplicated(values);
            if (report)
                SaveReported(tupple?.Item2 ?? new List<TaxiOrder>());
            return tupple?.Item2?.ToList()?.Count() == 0;
        }

        private Tuple<IEnumerable<TaxiOrder>, IEnumerable<TaxiOrder>> FilterDuplicated(IEnumerable<TaxiOrder> values)
        {
            //6.Identify and remove any duplicate records from the dataset based on a combination of 
            //    `pickup_datetime`, `dropoff_datetime`, and `passenger_count`. Write all removed duplicates into a `duplicates.csv` file.
            IDictionary<string, bool> hasDuplicates = new Dictionary<string, bool>();
            foreach (var item in values)
            {
                string key = $"{item.tpep_pickup_datetime.ToString()}-{item.tpep_dropoff_datetime.ToString()}-{item.passenger_count.ToString()}";
                if (hasDuplicates.ContainsKey(key))
                    hasDuplicates[key] = true;
                else
                    hasDuplicates.Add(key, false);
            }

            IList<TaxiOrder> validData = new List<TaxiOrder>();
            IList<TaxiOrder> invalidData = new List<TaxiOrder>();

            foreach (var item in values)
            {
                if (hasDuplicates[$"{item.tpep_pickup_datetime.ToString()}-{item.tpep_dropoff_datetime.ToString()}-{item.passenger_count.ToString()}"] == false)
                    validData.Add(item);
                else
                    invalidData.Add(item);
            }

            return new Tuple<IEnumerable<TaxiOrder>, IEnumerable<TaxiOrder>>(validData, invalidData);
        }

        private void SaveReported(IEnumerable<TaxiOrder> values)
        {
            using (var writer = new StreamWriter(reportDir))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(values);
            }
        }
    }
}
