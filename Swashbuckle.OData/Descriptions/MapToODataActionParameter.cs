using System.Linq;
using System.Web.Http.Controllers;
using System.Web.OData;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData.Descriptions
{
    internal class MapToODataActionParameter : IParameterMapper
    {
        public HttpParameterDescriptor Map(Parameter swaggerParameter, int parameterIndex, HttpActionDescriptor actionDescriptor)
        {
            var required = swaggerParameter.required;

            if (swaggerParameter.@in == "body" && swaggerParameter.schema != null && swaggerParameter.schema.type == "object")
            {
                var odataActionParametersDescriptor = actionDescriptor.GetParameters().SingleOrDefault(descriptor => descriptor.ParameterType == typeof (ODataActionParameters));
                return new ODataActionParameterDescriptor(odataActionParametersDescriptor.ParameterName, typeof(ODataActionParameters), !required.Value, swaggerParameter.schema, odataActionParametersDescriptor)
                {
                    Configuration = actionDescriptor.ControllerDescriptor.Configuration,
                    ActionDescriptor = actionDescriptor
                };
            }
            return null;
        }
    }
}