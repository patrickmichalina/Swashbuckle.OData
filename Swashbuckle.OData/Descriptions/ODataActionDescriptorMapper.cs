using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;

namespace Swashbuckle.OData.Descriptions
{
    internal class ODataActionDescriptorMapper : ODataActionDescriptorMapperBase, IODataActionDescriptorMapper
    {
        public IEnumerable<ApiDescription> Map(ODataActionDescriptor oDataActionDescriptor)
        {
            var apiDescriptions = new List<ApiDescription>();

            var apiDocumentation = GetApiDocumentation(oDataActionDescriptor.ActionDescriptor);

            var parameterDescriptions = CreateParameterDescriptions(oDataActionDescriptor.ActionDescriptor);

            PopulateApiDescriptions(oDataActionDescriptor, parameterDescriptions, apiDocumentation, apiDescriptions);

            return apiDescriptions;
        }

        private static string GetApiDocumentation(HttpActionDescriptor actionDescriptor)
        {
            var documentationProvider = actionDescriptor.Configuration.Services.GetDocumentationProvider();
            return documentationProvider?.GetDocumentation(actionDescriptor);
        }
    }
}