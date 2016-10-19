using System;
using System.Diagnostics.Contracts;
using System.Web.Http.Controllers;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData.Descriptions
{
    [ContractClass(typeof(ParameterMapperContract))]
    internal interface IParameterMapper
    {
        /// <summary>
        /// Map a Swagger parameter to a controller's HttpParameterDescriptor.
        /// </summary>
        /// <param name="swaggerParameter">The Swagger parameter to map.</param>
        /// <param name="parameterIndex">The index of the Swagger parameter.</param>
        /// <param name="actionDescriptor">A controller's action descriptor.</param>
        /// <returns>A non-null instance if successfully mapped, otherwise null.</returns>
        HttpParameterDescriptor Map(Parameter swaggerParameter, int parameterIndex, HttpActionDescriptor actionDescriptor);
    }

    [ContractClassFor(typeof(IParameterMapper))]
    internal abstract class ParameterMapperContract : IParameterMapper
    {
        public HttpParameterDescriptor Map(Parameter swaggerParameter, int parameterIndex, HttpActionDescriptor actionDescriptor)
        {
            throw new NotImplementedException();
        }
    }
}