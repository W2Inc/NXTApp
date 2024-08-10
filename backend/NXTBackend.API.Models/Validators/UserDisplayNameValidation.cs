﻿using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NXTBackend.API.Models.Validators;

/// <summary>
/// Attribute to validate and make sure that the string matches the display name constraints
/// </summary>
public partial class UserDisplayNameValidation : ValidationAttribute
{
    /// <inheritdoc/>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || value is not string displayName)
            return new ValidationResult("Invalid display name");

        var regex = DisplayNameMatching();

        if (!regex.IsMatch(displayName))
            return new ValidationResult("Invalid display name");

        return ValidationResult.Success;
    }

    [GeneratedRegex(@"^[a-zA-Z0-9]+(?:[_-][a-zA-Z0-9]+)*$", RegexOptions.Compiled)]
    private static partial Regex DisplayNameMatching();
}
