using ElasticServiceDDL.Method;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticServiceDDL
{
    public static class ElasticExtensions
    {
        public static IServiceCollection AddElastic(this IServiceCollection services, Action<ElasticConfigration> configurations)
        {

            if (configurations is null) throw new ArgumentNullException(nameof(configurations));
            var newOptions = new ElasticConfigration();
            configurations.Invoke(newOptions);

            services.AddSingleton(new ElasticClientProvider(newOptions));
            services.AddSingleton(typeof(ISearch<>),(typeof(Search<>)));
            services.AddSingleton(typeof(IHelperFunc),(typeof(HelperFunc)));

            return services;
        }
    }
}
