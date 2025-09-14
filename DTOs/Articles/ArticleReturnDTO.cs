namespace VHSKCD.DTOs.Articles
{
    public class ArticleReturnDTO
    {
        public int Id { get; set; }

        public string Thumbnail { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string Content { get; set; } = null!;

        public sbyte? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? ArticleCateId { get; set; }

        public string ArticleCateName { get; set; } = null!;
    }
}
