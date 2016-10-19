using System;
using System.Web.OData;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData
{
    internal static class SchemaExtensions
    {
        public static Type GetReferencedType(this Schema schema)
        {
            if (schema.@ref != null)
            {
                var fullTypeName = schema.@ref.Replace("#/definitions/", string.Empty);

                return TypeHelper.FindType(fullTypeName);
            }

            return null;
        }
    }
}