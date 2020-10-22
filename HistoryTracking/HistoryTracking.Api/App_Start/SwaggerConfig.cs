using System.Web.Http;
using WebActivatorEx;
using HistoryTracking.Api;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace HistoryTracking.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "Api Documentation");

                        /*var xmlFile = $"bin\\{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.XML";
                        var xmlPath = System.AppDomain.CurrentDomain.BaseDirectory + xmlFile;
                        c.IncludeXmlComments(xmlPath);*/

                        c.DescribeAllEnumsAsStrings();

                        c.IgnoreObsoleteActions();
                    })
                .EnableSwaggerUi(c =>
                    {
                        c.DocumentTitle("History Tracking Api Documentation");
                    });
        }
    }
}
