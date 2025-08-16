namespace Ambev.Ominia.Domain.Responses;

public class PaginatedResponse : Response
{
    public int CurrentPage { get; set; }

    public int TotalPages { get; set; }

    public int TotalCount { get; set; }

    public PaginatedResponse(object data, int currentPage = 1, int pageSize = 10)
    {
        Data = data;
        CurrentPage = currentPage;
        TotalCount = 0;
        TotalPages = 1;
    }
}