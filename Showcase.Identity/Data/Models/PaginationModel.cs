namespace Showcase.Identity.Data.Models;

public class PaginationModel 
{
    public int PageNumber { get; set; } = 1;
    
    public int PageSize { get; set; }
    
    public int TotalPageCount { get; set; } = 1;
}