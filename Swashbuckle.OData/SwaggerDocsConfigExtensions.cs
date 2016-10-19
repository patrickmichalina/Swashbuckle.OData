using System.Collections.Generic;
using System.Linq;
using Swashbuckle.Application;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData
{
    internal static class SwaggerDocsConfigExtensions
    {
        public static T GetFieldValue<T>(this SwaggerDocsConfig swaggerDocsConfig, string fieldName, bool ensureNonNull = false)
        {
            return swaggerDocsConfig.GetInstanceField<T>(fieldName, ensureNonNull);
        }

        public static Dictionary<string, SecurityScheme> GetSecurityDefinitions(this SwaggerDocsConfig swaggerDocsConfig)
        {
            var securitySchemeBuilders = swaggerDocsConfig.GetFieldValue<IDictionary<string, SecuritySchemeBuilder>>("_securitySchemeBuilders");

            return securitySchemeBuilders.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.InvokeFunction<SecurityScheme>("Build"));
        }
    }
}