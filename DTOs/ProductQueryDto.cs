namespace myFirstWebApi.DTOs;

public class ProductQueryDto
{
    public string? Search { get; set; }
    public int? CategoryId { get; set; }      // ← add this
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }
    public string SortOrder { get; set; } = "asc";

    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 50 ? 50 : value < 1 ? 1 : value;
    }
}