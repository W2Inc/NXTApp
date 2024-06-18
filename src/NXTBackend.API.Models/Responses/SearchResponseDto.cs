using System.ComponentModel.DataAnnotations;
using NXTBackend.API.Domain.Enums;

namespace NXTBackend.API.Models.Requests;

//public class SearchResult<T>
//{
//    public T? Data
//    {
//        get; set;
//    }
//}

public class SearchResponseDto<T> : BaseResponseDto
{
    public IReadOnlyCollection<T> Results { get; set; } = [];
}