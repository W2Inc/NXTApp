// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Microsoft.EntityFrameworkCore;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Jobs.Interface;
using Quartz;

// ============================================================================

namespace NXTBackend.API.Jobs;

/// <summary>
/// A job that computes the composition of a project's review.
/// Essentially projects are made up of multiple reviews.
/// </summary>
/// <param name="ctx">Database</param>
/// <param name="logger">Logger</param>
public class ReviewCompositionJob(ILogger<ReviewCompositionJob> logger, DatabaseContext ctx) : IScheduledJob
{
    /// <inheritdoc />
    public static string Identity => nameof(ReviewCompositionJob);

    /// <inheritdoc />
    public static string? Schedule => "0 * * ? * *";

    /// <inheritdoc />
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("{Job} is starting", Identity);
        // logger.LogInformation("{users}", await ctx.Users.ToListAsync(context.CancellationToken));

        await Task.CompletedTask;
    }
}
