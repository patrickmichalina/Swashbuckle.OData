using System.Collections;
using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData.Descriptions
{
    public static class ApiDescriptionExtensions
    {
        public static string DefaultGroupingKeySelector(this ApiDescription apiDescription)
        {
            return apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName == "Restier"
                ? ((RestierHttpActionDescriptor)apiDescription.ActionDescriptor).EntitySetName
                : apiDescription.ActionDescriptor.ControllerDescriptor.ControllerName;
        }

        public static string OperationId(this ApiDescription apiDescription)
        {
            return $"{apiDescription.DefaultGroupingKeySelector()}_{apiDescription.ActionDescriptor.ActionName}";
        }

        public static string GetRelativePathForSwagger(this ApiDescription apiDescription)
        {
            var parameters = apiDescription.ParameterDescriptions;

            var newRelativePathSansQueryString = apiDescription.RelativePathSansQueryString();
            newRelativePathSansQueryString = AdjustRelativePathForStringParams(parameters, newRelativePathSansQueryString);
            newRelativePathSansQueryString = AdjustRelativePathForArrayParams(parameters, newRelativePathSansQueryString);

            var relativePath = apiDescription.RelativePath;
            var oldRelativePathSansQueryString = apiDescription.RelativePathSansQueryString();
            return relativePath.Replace(oldRelativePathSansQueryString, newRelativePathSansQueryString);
        }

        private static string AdjustRelativePathForStringParams(IEnumerable<ApiParameterDescription> parameters, string newRelativePathSansQueryString)
        {
            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterDescriptor?.ParameterType;

                if (newRelativePathSansQueryString.Contains("{" + parameter.Name + "}") && parameterType == typeof (string))
                {
                    newRelativePathSansQueryString = newRelativePathSansQueryString.Replace("{" + parameter.Name + "}", "\'{" + parameter.Name + "}\'");
                }
            }
            return newRelativePathSansQueryString;
        }

        private static string AdjustRelativePathForArrayParams(IEnumerable<ApiParameterDescription> parameters, string newRelativePathSansQueryString)
        {
            foreach (var parameter in parameters)
            {
                var parameterType = parameter.ParameterDescriptor?.ParameterType;

                if (newRelativePathSansQueryString.Contains("{" + parameter.Name + "}")
                    && typeof(IEnumerable).IsAssignableFrom(parameterType)
                    && parameterType != typeof(string))
                {
                    newRelativePathSansQueryString = newRelativePathSansQueryString.Replace("{" + parameter.Name + "}", "[{" + parameter.Name + "}]");
                }
            }
            return newRelativePathSansQueryString;
        }
    }
}