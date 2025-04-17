// ============================================================================
// Copyright (c) 2024 - W2Wizard.
// See README.md in the project root for license information.
// ============================================================================

using Quartz;

// ============================================================================

namespace NXTBackend.API.Jobs.Interface;

/// <summary>
/// Base shape for a Scheduled Job.
/// </summary>
public interface IScheduledJob : IJob
{
    /// <summary>
    /// A cron schedule to determine how often to trigger the job.
    ///
    /// If null, the job may only be triggered manually via the trigger.
    /// You may also use <see cref="CronScheduleBuilder"/> if it's easier.
    /// </summary>
    public static abstract string? Schedule { get; }

    /// <summary>
    /// The name / identity of the job.
    /// </summary>
    public static abstract string Identity { get; }
}
