// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using NXTBackend.API.Models.Responses.Objects;
using System.Text.Json.Serialization;

// ============================================================================

namespace NXTBackend.API.Filters;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Category
{
    User,
    Project,
    Cursus,
    LearningGoal,
    Any
}


public class SearchResponseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Only apply this filter to the specific action or model that needs conditional response documentation
        if (context.Type == typeof(IEnumerable<object>)) // Assuming the Search response is IEnumerable<object>
        {
            // Define individual response schemas for each category
            var userSchema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<UserDO>), context.SchemaRepository);
            var projectSchema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<ProjectDO>), context.SchemaRepository);
            var cursusSchema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<CursusDO>), context.SchemaRepository);
            var learningGoalSchema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<LearningGoalDO>), context.SchemaRepository);

            // Combined schema for 'Any' which includes all types
            var anySchema = new OpenApiSchema
            {
                OneOf = [userSchema, projectSchema, cursusSchema, learningGoalSchema],
                Type = "array"
            };

            // Add individual schemas to oneOf
            var oneOfSchemas = new List<OpenApiSchema> { userSchema, projectSchema, cursusSchema, learningGoalSchema, anySchema }
                .Where(s => s != null)
                .ToList();

            // Set up the discriminator for distinguishing categories
            schema.Discriminator = new OpenApiDiscriminator
            {
                PropertyName = "category",
                Mapping = new Dictionary<string, string>
                {
                    { nameof(Category.User), nameof(UserDO) },
                    { nameof(Category.Project), nameof(ProjectDO) },
                    { nameof(Category.Cursus), nameof(CursusDO) },
                    { nameof(Category.LearningGoal), nameof(LearningGoalDO) },
                    { nameof(Category.Any), "Any" } // Map 'Any' to combined schema
                }
            };

            // Apply the oneOf property to the schema to represent the conditional response types
            schema.OneOf = oneOfSchemas;
            schema.Type = "array";
            schema.Items = null; // Remove any default items schema, as we're using oneOf instead
        }
    }
}
