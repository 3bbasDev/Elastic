using Elasticsearch.Net;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL
{
    public class ElasticClientProvider
    {
        private readonly Uri Uri;
        public readonly string Index;
        private readonly string UserName;
        private readonly string Password;
        public ElasticClient Client { get; }
        public ElasticClientProvider(ElasticConfigration configration)
        {
            Uri = configration.Uri;
            Index = configration.Index;
            UserName = configration.UserName;
            Password = configration.Password;

            Client = new ElasticClient(Generator());
        }
        private ConnectionSettings Generator()
        {
            var pool = new SingleNodeConnectionPool(Uri);
            var connectionSettings = new ConnectionSettings(pool)
                                   .DefaultIndex(Index)
                                   .BasicAuthentication(!String.IsNullOrEmpty(UserName) ? UserName : null, !String.IsNullOrEmpty(Password) ? Password : null)
                                   .DnsRefreshTimeout(TimeSpan.FromSeconds(100))
                                   .PrettyJson()
                                   .DisableDirectStreaming()
                                   
                                   .OnRequestCompleted(response =>
                                   {
                                       //if (response.Success)
                                       //{
                                       //    Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(response.RequestBodyInBytes));
                                       //    Console.WriteLine("---------------------------------------------------------------------");
                                       //    Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(response.ResponseBodyInBytes));
                                       //}
                                       if (response.RequestBodyInBytes == null)
                                       {
                                           throw new Exception($"{response.HttpMethod} {response.Uri}");

                                       }
                                       if (response.ResponseBodyInBytes == null)
                                       {
                                           throw new Exception($"Status: {response.HttpStatusCode}\n" +
                                                    $"{new string('-', 30)}\n");
                                       }
                                   });
            return connectionSettings;
            
        }
        
    }
}
