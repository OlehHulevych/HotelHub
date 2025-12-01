namespace server.models;

public class PaginatedItems<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages => TotalCount / 10;

    public PaginatedItems(List<T> Items, int TotalCount, int CurrentPage)
    {
        this.Items = Items;
        this.TotalCount = TotalCount;
        this.CurrentPage = CurrentPage;
        
    }
}