using System;
using System.Collections.Generic;

namespace ElasticApi.Models
{
    public class User
    {
        public string CustomerName { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string Email { get; set; }
        public List<User> Parent { get; set; }
        public static User FromCsv(string csvLine)
        {
            string[] values = csvLine.Split(',');
            User dailyValues = new User
            {
                CustomerName = Convert.ToString(values[0]),
                Email = Convert.ToString(values[1]),
                Phone1 = Convert.ToString(values[2]),
                Phone2 = Convert.ToString(values[3]),
                Parent=null
            };
            return dailyValues;
        }
    }
}
