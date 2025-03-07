namespace GoodMoodPerfumeBot.Models
{
    public class Product
    {
        int ProductId { get; set; }
        string? ProductName { get; set; }
        string? ProductDescription { get; set; }
        string? ProductUrls { get; set; }
        decimal ProductPrice { get; set; }
    }
}
