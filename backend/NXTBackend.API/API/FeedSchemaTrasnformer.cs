using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Models.Responses.Objects.FeedResponses;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace NXTBackend.API.Infrastructure.OpenApi
{
    public class FeedSchemaTransformer : IOpenApiSchemaTransformer
    {
        public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
        {
            // Handle the FeedDO type to set up polymorphic schema
            // if (context.JsonTypeInfo?.Type == typeof(FeedDO))
            // {
            //     // Set up the discriminator for the base FeedDO schema
            //     schema.Discriminator = new OpenApiDiscriminator
            //     {
            //         PropertyName = "feedKind"
            //     };

            //     // Ensure required properties are present
            //     if (schema.Required == null)
            //     {
            //         schema.Required = new HashSet<string>();
            //     }

            //     schema.Required.Add("feedKind");
            // }
            // // Handle derived types
            // else if (context.JsonTypeInfo?.Type == typeof(CompletedProjectFeedDO))
            // {
            //     AddAllOfReference(schema, typeof(FeedDO));

            //     // Add specific CommentFeedDO properties
            //     if (schema.Properties.ContainsKey("feedKind"))
            //     {
            //         schema.Properties["feedKind"].Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
            //         {
            //             new Microsoft.OpenApi.Any.OpenApiString(FeedKind.CompletedProject.ToString())
            //         };
            //     }
            // }
            // else if (context.JsonTypeInfo?.Type == typeof(NewUserFeedDO))
            // {
            //     AddAllOfReference(schema, typeof(FeedDO));

            //     // Add specific LikeFeedDO properties
            //     if (schema.Properties.ContainsKey("feedKind"))
            //     {
            //         schema.Properties["feedKind"].Enum = new List<Microsoft.OpenApi.Any.IOpenApiAny>
            //         {
            //             new Microsoft.OpenApi.Any.OpenApiString(FeedKind.NewUser.ToString())
            //         };
            //     }
            // }

            return Task.CompletedTask;
        }

        private void AddAllOfReference(OpenApiSchema schema, Type baseType)
        {
            // This sets up the allOf schema pattern similar to the YAML example
            if (schema.AllOf == null)
            {
                schema.AllOf = new List<OpenApiSchema>
                {
                    new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = baseType.Name
                        }
                    }
                };
            }
        }
    }
}
