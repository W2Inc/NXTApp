using NXTBackend.API.Core.Services.Interface;
using NXTBackend.API.Core.Services.Interface.Queues;
using NXTBackend.API.Domain.Enums;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Jobs.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Quartz;
using Resend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NXTBackend.API.Core.Notifications.Welcome;

namespace NXTBackend.API.Jobs;

[DisallowConcurrentExecution]
public class DispatchNotificationsJob2(DatabaseContext ctx, ILogger<DispatchNotificationsJob> logger, INotificationQueue queue) : IScheduledJob
{
    public static string? Schedule => "0 0/1 * ? * *";
    public static string Identity => nameof(DispatchNotificationsJob);

    public Task Execute(IJobExecutionContext context)
    {

    }
}
