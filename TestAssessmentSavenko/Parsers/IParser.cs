using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAssessmentSavenko.Parsers
{
    internal interface IParser<T>
    {
        IEnumerable<T> Parse(string value, Type? classMapType = null);
        IEnumerable<T> ParseFile(string path, Type? classMapType = null);
    }
}

