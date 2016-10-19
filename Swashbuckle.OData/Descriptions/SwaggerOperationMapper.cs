using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData.Descriptions
{
    internal class SwaggerOperationMapper : ODataActionDescriptorMapperBase, IODataActionDescriptorMapper
    {
        private readonly IEnumerable<IParameterMapper> _parameterMappers;

        public SwaggerOperationMapper(IEnumerable<IParameterMapper> parameterMappers)
        {
            _parameterMappers = parameterMappers;
        }

        public IEnumerable<ApiDescription> Map(ODataActionDescriptor oDataActionDescriptor)
        {
            var apiDescriptions = new List<ApiDescription>();

            var operation = oDataActionDescriptor.Operation;
            if (operation != null)
            {
                var apiDocumentation = GetApiDocumentation(oDataActionDescriptor.ActionDescriptor, operation);

                var parameterDescriptions = CreateParameterDescriptions(operation, oDataActionDescriptor.ActionDescriptor);

                PopulateApiDescriptions(oDataActionDescriptor, parameterDescriptions, apiDocumentation, apiDescriptions);
            }

            return apiDescriptions;
        }

        private List<ApiParameterDescription> CreateParameterDescriptions(Operation operation, HttpActionDescriptor actionDescriptor)
        {
            return operation.parameters?
                .Select((parameter, index) => GetParameterDescription(parameter, index, actionDescriptor))
                // Concat reflected parameter descriptors to ensure that parameters are not missed
                // e.g., parameters not described by or derived from the EDM model.
                .Concat(CreateParameterDescriptions(actionDescriptor))
                .Distinct(new ApiParameterDescriptionEqualityComparer())
                .ToList();
        }

        private ApiParameterDescription GetParameterDescription(Parameter parameter, int index, HttpActionDescriptor actionDescriptor)
        {
            var httpParameterDescriptor = GetHttpParameterDescriptor(parameter, index, actionDescriptor);

            return new SwaggerApiParameterDescription
            {
                ParameterDescriptor = httpParameterDescriptor,
                Name = httpParameterDescriptor.Prefix ?? httpParameterDescriptor.ParameterName,
                Documentation = GetApiParameterDocumentation(parameter, httpParameterDescriptor),
                SwaggerSource = parameter.MapToSwaggerSource(),
                Source = parameter.MapToApiParameterSource()
            };
        }

        private HttpParameterDescriptor GetHttpParameterDescriptor(Parameter parameter, int index, HttpActionDescriptor actionDescriptor)
        {
            var result = _parameterMappers
                .Select(mapper => mapper.Map(parameter, index, actionDescriptor))
                .FirstOrDefault(httpParameterDescriptor => httpParameterDescriptor != null);

            return result;
        }

        private static string GetApiParameterDocumentation(Parameter parameter, HttpParameterDescriptor parameterDescriptor)
        {
            var documentationProvider = parameterDescriptor.Configuration.Services.GetDocumentationProvider();

            return documentationProvider != null
                ? documentationProvider.GetDocumentation(parameterDescriptor)
                : parameter.description;
        }

        private static string GetApiDocumentation(HttpActionDescriptor actionDescriptor, Operation operation)
        {
            var documentationProvider = actionDescriptor.Configuration.Services.GetDocumentationProvider();
            return documentationProvider != null
                ? documentationProvider.GetDocumentation(actionDescriptor)
                : operation.description;
        }
    }
}