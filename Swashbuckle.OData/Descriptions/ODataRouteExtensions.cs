using System;
using System.Runtime.CompilerServices;
using System.Web.Http;
using System.Web.OData.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;

namespace Swashbuckle.OData.Descriptions
{
    internal static class ODataRouteExtensions
    {
        private static readonly ConditionalWeakTable<ODataRoute, HttpConfiguration> RouteConfigurationTable = new ConditionalWeakTable<ODataRoute, HttpConfiguration>();

        public static void SetHttpConfiguration(this ODataRoute oDataRoute, HttpConfiguration httpConfig)
        {
            HttpConfiguration registeredConfiguration;
            if (RouteConfigurationTable.TryGetValue(oDataRoute, out registeredConfiguration))
            {
            }
            else
            {
                RouteConfigurationTable.Add(oDataRoute, httpConfig);
            }
        }

        private static HttpConfiguration GetHttpConfiguration(this ODataRoute oDataRoute)
        {
            var result = RouteConfigurationTable.GetValue(oDataRoute, key => null);
            return result;
        }

        private static IServiceProvider GetRootContainer(this ODataRoute oDataRoute)
        {
            return oDataRoute.GetHttpConfiguration().GetODataRootContainer(oDataRoute);
        }

        public static IEdmModel GetEdmModel(this ODataRoute oDataRoute)
        {
            return oDataRoute.GetRootContainer().GetRequiredService<IEdmModel>();
        }

        public static bool IsEnumPrefixFree(this ODataRoute oDataRoute)
        {
            var uriResolver = oDataRoute.GetRootContainer().GetRequiredService<ODataUriResolver>();
            return uriResolver is StringAsEnumResolver;
        }

        public static string GetRoutePrefix(this ODataRoute oDataRoute)
        {
            return oDataRoute.RoutePrefix ?? string.Empty;
        }
    }
}