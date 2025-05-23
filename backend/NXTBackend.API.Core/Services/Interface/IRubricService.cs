﻿// ============================================================================
// W2Inc, Amsterdam 2023-2024, All Rights Reserved.
// See README in the root project for more information.
// ============================================================================

using NXTBackend.API.Core.Services.Traits;
using NXTBackend.API.Domain.Entities.Evaluation;

namespace NXTBackend.API.Core.Services.Interface;

/// <summary>
/// Service for the Rubric entity.
/// </summary>
public interface IRubricService : IDomainService<Rubric>, ICollaborative<Rubric>
{

}
