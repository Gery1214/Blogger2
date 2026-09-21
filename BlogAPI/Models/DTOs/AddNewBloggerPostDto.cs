namespace BlogAPI.Models.DTOs
{
    public class AddNewBloggerPostDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int blogId { get; set; }
    }
}
