using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL
{
    public class ElasticConfigration
    {
        public Uri Uri { get; set; }
        public string Index { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
