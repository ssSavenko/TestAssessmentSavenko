using CsvHelper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestAssessmentSavenko.Models;

namespace TestAssessmentSavenko.Validators
{
    internal interface IValidator<T>
    {
        bool Validate(T value, bool report = false);
        bool Validate(IEnumerable<T> values, bool report = false);
        IEnumerable<T> FilterValid(IEnumerable<T> values, bool report = false);

        string ReportDir { get; set; }
    }
}
