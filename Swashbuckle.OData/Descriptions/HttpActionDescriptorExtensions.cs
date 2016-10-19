﻿using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace Swashbuckle.OData.Descriptions
{
    internal static class HttpActionDescriptorExtensions
    {
        public static ResponseDescription CreateResponseDescription(this HttpActionDescriptor actionDescriptor)
        {
            var responseTypeAttribute = actionDescriptor.GetCustomAttributes<ResponseTypeAttribute>();
            var responseType = responseTypeAttribute?.Select(attribute => attribute.ResponseType).FirstOrDefault();

            return new ResponseDescription
            {
                DeclaredType = actionDescriptor.ReturnType,
                ResponseType = responseType,
                Documentation = actionDescriptor.GetApiResponseDocumentation()
            };
        }

        private static string GetApiResponseDocumentation(this HttpActionDescriptor actionDescriptor)
        {
            var documentationProvider = actionDescriptor.Configuration.Services.GetDocumentationProvider();
            return documentationProvider?.GetResponseDocumentation(actionDescriptor);
        }
    }
}