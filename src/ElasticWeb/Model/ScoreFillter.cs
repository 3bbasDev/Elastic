using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticWeb.Model
{
    public class ScoreFillter
    {
        public int CustomerName { get; set; } =50;
        public int Phone1 { get; set; } =50;
        public int Phone2 { get; set; } =50;
        public int Email { get; set; } =50;
    }
}
