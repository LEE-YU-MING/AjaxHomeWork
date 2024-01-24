namespace WebApplication1.Models.Dtos
{
    public class UserDto
    {
        public string? Name { get; set; }
        public int? Age { get; set; } = 18;
        public string? Email { get; set; }
        public string? Password { get; set; }
        public IFormFile? Image { get; set; }
        //Avatar
    }
}
