using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL.Method
{
    public interface IHelperFunc
    {
        dynamic ConvertToDynamic(string[] Key, string[] Value);
        dynamic ConvertToDynamic(string[] Key, string[] Value, List<dynamic> Parent);

    }
}
