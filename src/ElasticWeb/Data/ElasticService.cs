using ElasticServiceDDL.Model;
using ElasticWeb.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ElasticWeb.Data
{
    public class ElasticService
    {
        public async Task<List<User>> CreateUser(User user)
        {

            var http = new HttpClient();
            RequstSearch<User,User> requstUser = new RequstSearch<User, User>()
            {
                Query="",
                Model=user,
                Page=1,
                PageSize=15
            };
            List<User> Users = new List<User>();
            string result;

            //using (var request = new HttpRequestMessage(HttpMethod.Post, "http://10.4.13.118:9966/api/elastic"))
            using (var request = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:5000/api/dynamic"))
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(requstUser), Encoding.UTF8, "application/json");
                var response = await http.SendAsync(request);
                result = await response.Content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Users;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    return JsonConvert.DeserializeObject<List<User>>(result);
                }
                else
                {
                    return null;
                }

            };
        }
    }
}
