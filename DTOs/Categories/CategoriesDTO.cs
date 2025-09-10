using VHSKCD.Models;

namespace VHSKCD.DTOs.Categories
{
    public class CategoriesDTO
    {
        //public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
