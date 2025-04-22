// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using System.Security.Cryptography.X509Certificates;
using NXTBackend.API.Domain.Entities.Users;
using NXTBackend.API.Domain.Enums;

// ============================================================================

namespace NXTBackend.API.Domain.Common;

/// <summary>
/// Represents an entity that can own resources in the system.
/// Could be a User, an Organization, or another entity type.
/// </summary>
public record ResourceOwner(User? User, Guid? Organization, OwnerKind Type);
