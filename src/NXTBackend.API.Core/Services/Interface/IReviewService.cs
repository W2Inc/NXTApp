namespace NXTBackend.API.Core.Services.Interface;

using NXTBackend.API.Common;
using NXTBackend.API.Domain.Entities;
using NXTBackend.API.Infrastructure.Database;
using NXTBackend.API.Models.Requests;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface IReviewService : IDomainService<Review>
{

}