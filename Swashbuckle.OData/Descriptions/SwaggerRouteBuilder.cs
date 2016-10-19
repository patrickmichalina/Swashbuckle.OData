using System;
using System.Net.Http;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData.Descriptions
{
    public class SwaggerRouteBuilder
    {
        private readonly SwaggerRoute _swaggerRoute;

        internal SwaggerRouteBuilder(SwaggerRoute swaggerRoute)
        {
            _swaggerRoute = swaggerRoute;
        }

        public SwaggerRoute SwaggerRoute
        {
            get
            {
                return _swaggerRoute;
            }
        }

        public OperationBuilder Operation(HttpMethod httpMethod)
        {
            var operation = new Operation();

            switch (httpMethod.Method.ToUpper())
            {
                case "GET":
                    SwaggerRoute.PathItem.get = operation;
                    break;
                case "PUT":
                    SwaggerRoute.PathItem.put = operation;
                    break;
                case "POST":
                    SwaggerRoute.PathItem.post = operation;
                    break;
                case "DELETE":
                    SwaggerRoute.PathItem.delete = operation;
                    break;
                case "PATCH":
                case "MERGE":
                    SwaggerRoute.PathItem.patch = operation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(httpMethod));
            }

            return new OperationBuilder(operation, this);
        }

        public Operation GetOperation(HttpMethod httpMethod)
        {
            switch (httpMethod.Method.ToUpper())
            {
                case "GET":
                    return SwaggerRoute.PathItem.get;
                case "PUT":
                    return SwaggerRoute.PathItem.put;
                case "POST":
                    return SwaggerRoute.PathItem.post;
                case "DELETE":
                    return SwaggerRoute.PathItem.delete;
                case "PATCH":
                case "MERGE":
                    return SwaggerRoute.PathItem.patch;
                default:
                    throw new ArgumentOutOfRangeException(nameof(httpMethod));
            }
        }
    }
}