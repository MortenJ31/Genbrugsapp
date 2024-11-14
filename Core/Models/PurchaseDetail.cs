namespace Core.Models
{
    public class PurchaseDetail
    {
        public string Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Status { get; set; }
        public string LocationId { get; set; }

        // Ad-oplysninger
        public string AdTitle { get; set; }
        public string AdDescription { get; set; }
        public double AdPrice { get; set; }
        public string AdImageUrl { get; set; }

        public Ad AdDetails { get; set; }
    }
}
