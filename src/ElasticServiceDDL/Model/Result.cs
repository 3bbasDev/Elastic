using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL.Model
{
    public class Result
    {
        public dynamic Header { get; set; }
        public List<ModelInfo> Model { get; set; }
    }
}
