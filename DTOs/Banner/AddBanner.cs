namespace VHSKCD.DTOs.Banner
{
    public class AddBanner
    {
        public string Title { get; set; } = null!;

        public string? Link { get; set; }

        public sbyte? Status { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
