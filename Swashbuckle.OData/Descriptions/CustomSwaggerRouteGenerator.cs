﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Routing;

namespace Swashbuckle.OData.Descriptions
{
    internal class CustomSwaggerRouteGenerator : ISwaggerRouteGenerator
    {
        public IEnumerable<SwaggerRoute> Generate(HttpConfiguration httpConfig)
        {
            return httpConfig.GetODataRoutes().SelectMany(oDataRoute => GetCustomSwaggerRoutes(httpConfig, oDataRoute));
        }

        public static List<SwaggerRoute> GetCustomSwaggerRoutes(HttpConfiguration httpConfig, ODataRoute oDataRoute)
        {
            object swaggerRoutes;
            httpConfig.Properties.TryGetValue(oDataRoute, out swaggerRoutes);

            return swaggerRoutes as List<SwaggerRoute> ?? new List<SwaggerRoute>();
        }
    }
}