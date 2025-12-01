namespace server.DTO;

public class PaginationDTO
{
    public List<Object> items { get; set; }
    public int currentPage { get; set; }
    public int totalPages { get; set; }
    public string type { get; set; }
    
}