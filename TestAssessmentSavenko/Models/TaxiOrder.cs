using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssessmentSavenko.Models
{
    internal class TaxiOrder
    {
        public DateTime tpep_pickup_datetime { get; set; }
        public DateTime tpep_dropoff_datetime { get; set; }
        public int passenger_count { get; set; }
        public double? trip_distance { get; set; }
        public string store_and_fwd_flag { get; set;}
        public int? PULocationID { get; set; }
        public int? DOLocationID { get; set; }
        public decimal fare_amount { get; set; }
        public decimal tip_amount { get; set; }
    }


    internal class TaxiOrderMap : ClassMap<TaxiOrder>
    {
        public TaxiOrderMap()
        {
            Map(m => m.passenger_count).TypeConverterOption.NullValues("").Default(0);
            Map(m => m.trip_distance).TypeConverterOption.NullValues("").Default(0.0);
            Map(m => m.PULocationID).TypeConverterOption.NullValues("").Default(0);
            Map(m => m.DOLocationID).TypeConverterOption.NullValues("").Default(0);

            Map(m => m.store_and_fwd_flag).Convert(row =>
            {
                //7. For the `store_and_fwd_flag` column, convert any 'N' values to 'No' and any 'Y' values to 'Yes'.
                //There is no point where should I do it so I implemneted it on parsing stage: 
                var value = row.Row.GetField("store_and_fwd_flag");
                //8. Ensure that all text-based fields are free from leading or trailing whitespace.
                return value.Trim() == "Y" ? "Yes" : "No";
            });

            Map(m => m.tpep_pickup_datetime);
            Map(m => m.tpep_dropoff_datetime);
            Map(m => m.fare_amount);
            Map(m => m.tip_amount);
        }
    }
}
