// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

// ============================================================================

internal sealed class InfoSchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider) : IOpenApiDocumentTransformer
{
    public async Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        document.Info = new()
        {
            Title = "NXT API",
            Version = "v1",
            Description = "The NXT API provides access to NXT's data. THis way you can programmatically subscribe a project and more!",
            Contact = new OpenApiContact
            {
                Email = "info@nextdemy.com",
                Name = "W2Wizard"
            }
        };
    }
}
