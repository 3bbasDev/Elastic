using ElasticServiceDDL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;

namespace ElasticApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddElastic(f => {

                f.Index = "recipes";
                f.Uri = new Uri("http://localhost:9200");
                f.UserName = "elastic";
                f.Password = "3YMmkrrSXjERnsKem0td";
            }) ;

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.Configure<ApiBehaviorOptions>(c => c.SuppressModelStateInvalidFilter = true);
            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app)
        {

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
