namespace Core.Models
{
    public class PurchaseDetail
    {
        public string Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; }
        public string LocationId { get; set; }

        // Ad-oplysninger
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
    }
}
