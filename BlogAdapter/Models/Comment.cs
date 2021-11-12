using System;

namespace BlogAdapter.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string PostId { get; set; }
        public string Text { get; set; }
    }
}
