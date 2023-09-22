using Microsoft.Extensions.Hosting;

namespace RaspberryPi.API.Models.Data
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new();
    }
}