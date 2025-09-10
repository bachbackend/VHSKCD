namespace VHSKCD.DTOs.Articles
{
    public class ArticlesDTO
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public string Title { get; set; }
        public string Thumbnail { get; set; }
        public sbyte? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UserId { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
