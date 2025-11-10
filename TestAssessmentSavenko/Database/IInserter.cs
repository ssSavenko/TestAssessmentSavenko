using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssessmentSavenko.Database
{
    internal interface IInserter<T>
    {
        void Insert(T entity);
        void Insert(IEnumerable<T> entities);

        string ConnectionString { get; set; }
        int BulkSize { get; set; }
    }
}
