using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL.Method
{
    public class HelperFunc : IHelperFunc
    {
        public dynamic ConvertToDynamic(string[] Key, string[] Value)
        {
            dynamic item = new ExpandoObject();
            var dItem = item as IDictionary<string, object>;
            for (int i = 0; i < Key.Length; i++)
            {
                dItem.Add(Key[i], Value[i]);
            }

            return item;
        }

        public dynamic ConvertToDynamic(string[] Key, string[] Value, List<dynamic> Parent)
        {
            dynamic item = new ExpandoObject();
            var dItem = item as IDictionary<string, object>;
            for (int i = 0; i < Key.Length; i++)
            {
                dItem.Add(Key[i], Value[i]);
            }
            dItem.Add("parent", Parent);
            return item;
        }
    }
}
