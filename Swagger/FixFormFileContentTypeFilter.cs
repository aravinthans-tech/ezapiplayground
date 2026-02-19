using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace QRCodeAPI.Swagger;

// Parameter filter to prevent ContentType from being added for IFormFile parameters
public class FormFileParameterFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        // For IFormFile parameters, ensure they're treated as file uploads without ContentType
        if (context.ParameterInfo?.ParameterType == typeof(IFormFile) ||
            context.ParameterInfo?.ParameterType == typeof(Microsoft.AspNetCore.Http.IFormFile))
        {
            parameter.Schema = new OpenApiSchema
            {
                Type = "string",
                Format = "binary"
            };
        }
    }
}

// Operation filter to fix duplicate ContentType in request body
public class FixFormFileContentTypeFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Fix duplicate ContentType issue for multipart/form-data endpoints
        if (operation.RequestBody?.Content?.ContainsKey("multipart/form-data") == true)
        {
            var formData = operation.RequestBody.Content["multipart/form-data"];
            if (formData?.Schema?.Properties != null)
            {
                // Remove all ContentType properties (they're not needed for file uploads)
                var keysToRemove = formData.Schema.Properties.Keys
                    .Where(k => k.Contains("ContentType", System.StringComparison.OrdinalIgnoreCase))
                    .ToList();

                foreach (var key in keysToRemove)
                {
                    formData.Schema.Properties.Remove(key);
                }
            }
        }
    }
}

