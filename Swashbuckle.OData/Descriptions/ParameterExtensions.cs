﻿using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace Swashbuckle.OData.Descriptions
{
    internal static class ParameterExtensions
    {
        public static ParameterSource MapToSwaggerSource(this Parameter parameter)
        {
            switch (parameter.@in)
            {
                case "query":
                    return ParameterSource.Query;
                case "header":
                    return ParameterSource.Header;
                case "path":
                    return ParameterSource.Path;
                case "body":
                    return ParameterSource.Body;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
            }
        }

        public static ApiParameterSource MapToApiParameterSource(this Parameter parameter)
        {
            switch (parameter.@in)
            {
                case "query":
                    return ApiParameterSource.FromUri;
                case "header":
                    return ApiParameterSource.Unknown;
                case "path":
                    return ApiParameterSource.FromUri;
                case "body":
                    return ApiParameterSource.FromBody;
                default:
                    throw new ArgumentOutOfRangeException(nameof(parameter), parameter, null);
            }
        }

        public static Type GetClrType(this Parameter parameter)
        {
            var type = parameter.type;
            var format = parameter.format;

            switch (format)
            {
                case null:
                    switch (type)
                    {
                        case null:
                            Contract.Assume(parameter.@in.Equals(@"body"));
                            return !string.IsNullOrWhiteSpace(parameter.schema?.@ref) ? GetEntityTypeForBodyParameter(parameter) : typeof (object);
                        case "string":
                            return typeof(string);
                        case "boolean":
                            return typeof(bool);
                        default:
                            throw new Exception($"Could not determine .NET type for parameter type {type} and format 'null'");
                    }
                case "byte":
                    return typeof(byte);
                case "int32":
                    return typeof(int);
                case "int64":
                    return typeof(long);
                case "date":
                    return typeof(DateTime);
                case "date-time":
                    return typeof(DateTimeOffset);
                case "double":
                    return typeof(double);
                case "decimal":
                    return typeof(decimal);
                case "float":
                    return typeof(float);
                case "guid":
                    return typeof(Guid);
                case "binary":
                    return typeof(byte[]);
                default:
                    throw new Exception($"Could not determine .NET type for parameter type {type} and format {format}");
            }
        }

        public static string GenerateSamplePathParameterValue(this Parameter parameter)
        {
            var type = parameter.type;
            var format = parameter.format;

            switch (format)
            {
                case null:
                    switch (type)
                    {
                        case "string":
                            if (parameter.@enum != null && parameter.@enum.Any())
                            {
                                return parameter.@enum.First().ToString();
                            }
                            return "\'SampleString\'";
                        case "boolean":
                            return "true";
                        case "array":
                            return "[]";
                        default:
                            throw new Exception($"Could not generate sample value for query parameter type {type} and format {"null"}");
                    }
                case "byte":
                    return "254";
                case "int32":
                case "int64":
                    return "42";
                case "date":
                    return "2015-12-12T12:00";
                case "date-time":
                    return "2015-10-10T17:00:00Z";
                case "double":
                    return "2.34";
                case "decimal":
                    return "1.12";
                case "float":
                    return "2.0";
                case "guid":
                    return Guid.NewGuid().ToString();
                case "binary":
                    return Convert.ToBase64String(new byte[] { 130, 200, 234, 23 });
                default:
                    throw new Exception($"Could not generate sample value for query parameter type {type} and format {format}");
            }
        }

        private static Type GetEntityTypeForBodyParameter(Parameter parameter)
        {
            return parameter.schema.GetReferencedType();
        }
    }
}
