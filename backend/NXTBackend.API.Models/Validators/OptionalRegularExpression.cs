using System.ComponentModel.DataAnnotations;

namespace NXTBackend.API.Models.Validators;


public class OptionalRegularExpressionAttribute(string pattern) : RegularExpressionAttribute(pattern)
{
    public override bool IsValid(object? value)
    {
        Console.WriteLine($"\n\n\n\n\nVALUE: {value}, IsNull: {value is null}, IsString: {value is string}, Length: {value?.ToString()?.Length}");

        if (value is null || (value is string stringValue && string.IsNullOrEmpty(stringValue)))
            return true;
        return base.IsValid(value);
    }
}
