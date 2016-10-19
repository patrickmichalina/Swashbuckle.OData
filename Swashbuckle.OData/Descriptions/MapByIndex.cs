using System.Web.Http.Controllers;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData.Descriptions
{
    internal class MapByIndex : IParameterMapper
    {
        public HttpParameterDescriptor Map(Parameter swaggerParameter, int parameterIndex, HttpActionDescriptor actionDescriptor)
        {
            if (swaggerParameter.@in != "query" && parameterIndex < actionDescriptor.GetParameters().Count)
            {
                var parameterDescriptor = actionDescriptor.GetParameters()[parameterIndex];
                if (parameterDescriptor != null && !parameterDescriptor.IsODataLibraryType())
                {
                    var httpControllerDescriptor = actionDescriptor.ControllerDescriptor;
                    return new ODataParameterDescriptor(swaggerParameter.name, parameterDescriptor.ParameterType, parameterDescriptor.IsOptional, parameterDescriptor)
                    {
                        Configuration = httpControllerDescriptor.Configuration,
                        ActionDescriptor = actionDescriptor,
                        ParameterBinderAttribute = parameterDescriptor.ParameterBinderAttribute
                    };
                }
            }
            return null;
        }
    }
}