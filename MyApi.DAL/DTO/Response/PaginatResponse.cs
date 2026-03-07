using System;

namespace MyApi.DAL.DTO.Response;

public class PaginatResponse<T>
{
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public List<T> Data { get; set; }
}
