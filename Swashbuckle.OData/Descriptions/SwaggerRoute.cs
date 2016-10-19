using System.Web;
using System.Web.OData.Routing;
using Flurl;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData.Descriptions
{
    public class SwaggerRoute
    {
        private readonly string _template;

        public SwaggerRoute(string template, ODataRoute oDataRoute, PathItem pathItem)
        {
            _template = template;
            ODataRoute = oDataRoute;
            PathItem = pathItem;
        }

        public SwaggerRoute(string template, ODataRoute oDataRoute) :
            this(template, oDataRoute, new PathItem())
        {
        }

        public string Template => _template;

        public string PrefixedTemplate => HttpUtility.UrlDecode(ODataRoute.GetRoutePrefix().AppendPathSegment(_template));

        public ODataRoute ODataRoute { get; }

        public PathItem PathItem { get; }
    }
}