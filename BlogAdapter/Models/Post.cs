using System;

namespace BlogAdapter.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime PublishDate { get; set; }
        public int AuthorId { get; set; }
        public string Status { get; set; }
    }
}
