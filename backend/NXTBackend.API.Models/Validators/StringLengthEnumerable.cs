using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace NXTBackend.API.Models.Validators;


/// <summary>
/// Validates that all string elements in an enumerable do not exceed a specified maximum length.
/// </summary>
/// <remarks>
/// This attribute is designed to be used on properties that are collections of strings,
/// such as List&lt;string&gt; or string[], to ensure that each individual string element
/// in the collection does not exceed the specified maximum length.
/// </remarks>
/// <param name="maxLength">The maximum allowed length for each string in the enumerable.</param>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class StringLengthEnumerable : ValidationAttribute
{
	public int MinLength { get; init; }
	public int MaxLength { get; init; }

	public StringLengthEnumerable(int maxLength)
	{
		MaxLength = maxLength;
		MinLength = 0;
	}

	public StringLengthEnumerable(int minLength, int maxLength)
	{
		MinLength = minLength;
		MaxLength = maxLength;
	}

	/// <inheritdoc/>
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		if (value is not IEnumerable<string> items)
			return new ValidationResult("Element is not a enumerable string");
		
		foreach (string item in items)
			if (item.Length < MinLength || item.Length > MaxLength)
				return new ValidationResult($"Item '{item}' length must be between {MinLength} and {MaxLength} characters");
		return ValidationResult.Success;
	}
}