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
}


public class SearchResponseSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        // Only apply this filter to the specific action or model that needs conditional response documentation
        if (context.Type == typeof(IEnumerable<object>)) // Assuming the Search response is IEnumerable<object>
        {
            // Define the possible response types for each Category value
            var oneOfSchemas = new List<OpenApiSchema>();

            // Define each schema with the relevant model type and add it to oneOf
            var userSchema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<UserDO>), context.SchemaRepository);
            if (userSchema != null)
            {
                oneOfSchemas.Add(userSchema);
            }

            var projectSchema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<ProjectDO>), context.SchemaRepository);
            if (projectSchema != null)
            {
                oneOfSchemas.Add(projectSchema);
            }

            var cursusSchema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<CursusDO>), context.SchemaRepository);
            if (cursusSchema != null)
            {
                oneOfSchemas.Add(cursusSchema);
            }

            var learningGoalSchema = context.SchemaGenerator.GenerateSchema(typeof(IEnumerable<LearningGoalDO>), context.SchemaRepository);
            if (learningGoalSchema != null)
            {
                oneOfSchemas.Add(learningGoalSchema);
            }

            // Apply the oneOf property to the schema to represent the conditional response types
            schema.OneOf = oneOfSchemas;
            schema.Type = "array";
            schema.Items = null; // Remove any default items schema, as we're using oneOf instead
        }
    }
}
