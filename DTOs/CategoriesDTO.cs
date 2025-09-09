using VHSKCD.Models;

namespace VHSKCD.DTOs
{
    public class CategoriesDTO
    {
        public string Name { get; set; } = null!;

        public int? ParentId { get; set; }

        public DateTime? CreatedAt { get; set; }
        
    }
}
