namespace NXTBackend.API.Models.Responses;

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