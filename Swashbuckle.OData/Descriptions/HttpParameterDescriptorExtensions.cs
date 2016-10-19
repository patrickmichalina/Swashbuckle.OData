﻿using System.Diagnostics.Contracts;
using System.Web.Http.Controllers;
using System.Web.OData;

namespace Swashbuckle.OData.Descriptions
{
    internal static class HttpParameterDescriptorExtensions
    {
        public static bool IsODataLibraryType(this HttpParameterDescriptor parameterDescriptor)
        {
            return IsODataODataActionParameters(parameterDescriptor) || IsODataQueryOptions(parameterDescriptor);
        }

        private static bool IsODataODataActionParameters(this HttpParameterDescriptor parameterDescriptor)
        {
            var parameterType = parameterDescriptor.ParameterType;
            Contract.Assume(parameterType != null);

            return parameterType == typeof(ODataActionParameters);
        }

        private static bool IsODataQueryOptions(this HttpParameterDescriptor parameterDescriptor)
        {
            var parameterType = parameterDescriptor.ParameterType;
            Contract.Assume(parameterType != null);

            return parameterType.Name == "ODataQueryOptions`1";
        }
    }
}