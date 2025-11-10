using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessmentSavenko.Models;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TestAssessmentSavenko.Database
{
    internal class TaxiOrderInserter : IInserter<TaxiOrder>
    {
        string connectionString;
        int bulkSize = 1000;
        public TaxiOrderInserter(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public TaxiOrderInserter(string connectionString, int bulkSize)
        {
            this.connectionString = connectionString;
            this.bulkSize = bulkSize;
        }

        public string ConnectionString { get => connectionString; set => connectionString = value; }
        public int BulkSize { get => bulkSize; set => bulkSize = value; }

        public void Insert(TaxiOrder entity)
        {
            Insert(new List<TaxiOrder>() { entity });
        }

        public void Insert(IEnumerable<TaxiOrder> entities)
        {
            //1.Import the data from the CSV into an MS SQL table.We only want to store the following columns:
            //    - `tpep_pickup_datetime`
            //    - `tpep_dropoff_datetime`
            //    - `passenger_count`
            //    - `trip_distance`
            //    - `store_and_fwd_flag`
            //    - `PULocationID`
            //    - `DOLocationID`
            //    - `fare_amount`
            //    - `tip_amount`

            //5. Implement efficient bulk insertion of the processed records into the database.

            var table = new DataTable();
            table.Columns.Add("tpep_pickup_datetime", typeof(DateTime));
            table.Columns.Add("tpep_dropoff_datetime", typeof(DateTime));
            table.Columns.Add("passenger_count", typeof(int));
            table.Columns.Add("trip_distance", typeof(decimal));
            table.Columns.Add("store_and_fwd_flag", typeof(string));
            table.Columns.Add("PULocationID", typeof(int));
            table.Columns.Add("DOLocationID", typeof(int));
            table.Columns.Add("fare_amount", typeof(decimal));
            table.Columns.Add("tip_amount", typeof(decimal));

            //10. (nice to have) The input data is in the EST timezone.
            //Convert it to UTC when inserting into the DB.

            var estZone = TimeZoneInfo.FindSystemTimeZoneById("EST");
            foreach (var order in entities)
            {
                table.Rows.Add(
                    TimeZoneInfo.ConvertTimeToUtc(order.tpep_pickup_datetime, estZone),
                    TimeZoneInfo.ConvertTimeToUtc(order.tpep_dropoff_datetime, estZone),
                    order.passenger_count,
                    order.trip_distance,
                    order.store_and_fwd_flag,
                    order.PULocationID,
                    order.DOLocationID,
                    order.fare_amount,
                    order.tip_amount
                );
            }

            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var bulkCopy = new SqlBulkCopy(connection)
            {
                DestinationTableName = "TaxiOrders",
                BatchSize = bulkSize
            };

            bulkCopy.WriteToServer(table);
        }
    }
}
