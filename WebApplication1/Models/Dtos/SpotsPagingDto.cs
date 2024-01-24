namespace WebApplication1.Models.Dtos
{
    public class SpotsPagingDto
    {
        public int TotalPages { get; set; }
        public IEnumerable<SpotImagesSpot>? SpotResult { get; set; }
    }
}
