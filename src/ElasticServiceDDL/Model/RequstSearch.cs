using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL.Model
{
    public class RequstSearch<T,TS> where T : class where TS:class
    {
        public string Query { get; set; }
        public T Model { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public TS ScoreFillter { get; set; }
    }
}
