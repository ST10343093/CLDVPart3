namespace CLDVPart3.Models
{
    public class OrderAdminViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string UserEmail { get; set; }
        public string? Status { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
