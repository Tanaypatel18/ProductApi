namespace myFirstWebApi.DTOs;

public class ProductQueryDto
{
    public string? Search { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? SortBy { get; set; }      // name, price, stock
    public string SortOrder { get; set; } = "asc";  // asc, desc

    // Pagination
    private int _page = 1;
    private int _pageSize = 10;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;  // minimum page is 1
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 50 ? 50 : value < 1 ? 1 : value; // max 50, min 1
    }
}