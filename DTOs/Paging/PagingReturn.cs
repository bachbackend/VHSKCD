namespace VHSKCD.DTOs.Paging
{
    public class PagingReturn
    {
        public int TotalPageCount { get; set; }
        public int CurrentPage { get; set; }
        public int NextPage { get; set; }
        public int PreviousPage { get; set; }
    }
}
