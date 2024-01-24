namespace WebApplication1.Models.Dtos
{
    public class SearchDto
    {
        //搜尋相關
        public string? keyword { get; set; }
        public int? categoryId { get; set; } = 0;
        //0表示不跟據分類進行搜尋

        //排序相關
        public string? sortBy { get; set; }
        public string? sortType { get; set; } = "asc";

        //分頁相關
        public int? Page { get; set; } = 1;
        public int? pageSize { get; set; } = 9;
    }
}
